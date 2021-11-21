import router from '@/router';
import { Route } from 'vue-router';
import { OktaAuth, AccessToken, IDToken } from '@okta/okta-auth-js';

const ISSUER = 'https://dev-269240.oktapreview.com/oauth2/default' || process.env.ISSUER;
const CLIENT_ID = '0oat407tpxa7eUUWo0h7' || process.env.CLIENT_ID;
const REDIRECT_URL = window.location.origin + '/implicit/callback';



const oktaAuth: OktaAuth = new OktaAuth({
	issuer: ISSUER,
	clientId: CLIENT_ID,
	redirectUri: REDIRECT_URL,
});

/**
 * function which validate if there is any token available
 * @param next callback
 */
export function validateAccess(to: Route, from: Route, next: () => void) {
	try {
		getIdToken()
		.then((token) => {
			if (token) {
				next();
			} else {
				oktaAuth.tokenManager.clear();
				loginOkta();
			}
		})
		.catch((e) => {
			oktaAuth.tokenManager.clear();
			loginOkta();
		});
	} catch (e) {
		oktaAuth.tokenManager.clear();
		loginOkta();
	}
}

/**
 * Handler for the "Login initiated by Okta" feature of the app.
 * This is used to add an icon for opening the app to the Okta user home screen.
 * @author MH
 */
export function loginOkta() {
	oktaAuth.token.getWithRedirect({
		scopes: ['openid', 'profile', 'email'],
		prompt: 'login consent', // Determines whether the Okta login will be displayed on failure. default none
	});
}

/**
 * Logout from the current session
 */
export function logout() {
	getIdToken()
	.then((token) => {
		if (token) {
			const idToken = (token as IDToken).idToken;
			oktaAuth.tokenManager.clear();
			oktaAuth.session.close();
			window.location.href = ISSUER + '/v1/logout?client_id=' + CLIENT_ID +
				'&id_token_hint=' + idToken + '&post_logout_redirect_uri=' + window.location.origin;
		} else {
			router.push('/');
		}
	});
}

/**
 * function which will handle the authentication with okta
 */
export function callback() {
	try {
		// detect code
		oktaAuth.token.parseFromUrl()
		.then((res) => {
			// manage token or tokens
			const token = res.tokens;
			if (token.idToken) {
				oktaAuth.tokenManager.add('id_token', token.idToken);
			}
			if (token.accessToken) {
				oktaAuth.tokenManager.add('access_token', token.accessToken);
			}
			oktaAuth.token.getUserInfo(token.accessToken as AccessToken, token.idToken as IDToken).then((user) => {
				sessionStorage.setItem('user', JSON.stringify(user)); // store user on sessionStorage to access in other places
				router.push('/');
			});
		})
		.catch((e) => {
			oktaAuth.tokenManager.clear();
			oktaAuth.session.close();
			loginOkta();
		});
	 } catch (e) {
		oktaAuth.tokenManager.clear();
		oktaAuth.session.close();
		loginOkta();
	 }
}

export function getIdToken() {
	return oktaAuth.tokenManager.get('id_token');
}

export function getAccessToken() {
	return oktaAuth.tokenManager.get('access_token');
}
