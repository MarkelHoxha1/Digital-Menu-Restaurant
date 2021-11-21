import Vue from 'vue';
import VueRouter from 'vue-router';
import Menu from '@/views/Menu.vue';
Vue.use(VueRouter);
const routes = [
    {
        path: '/',
        name: 'home',
        component: Menu,
    },
    // {
    // 	path: '/implicit/callback',
    // 	component: callback,
    // },
    // {
    // 	path: '/dish/:uuid',
    // 	name: 'singlenews',
    // 	component: SingleNews,
    // 	// meta: { requiresAuth: true }, // Okta
    // 	// beforeEnter: validateAccess,
    // },
    {
        path: '*',
        redirect: '/',
    },
];
const router = new VueRouter({
    mode: 'history',
    routes,
});
export default router;
//# sourceMappingURL=index.js.map