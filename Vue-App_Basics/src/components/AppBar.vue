<template>
<v-app-bar app clipped-right dark color="#01426A" fixed extension-height="60">
	<v-btn icon @click="loadFile" title="Load dishes"><v-icon>mdi-cloud-download</v-icon></v-btn>

	<v-divider vertical class="mx-3"></v-divider>

	<v-btn icon to="menu" title="See Menu" class="noDecorate"><v-icon>mdi-open-in-new</v-icon></v-btn>

	<!-- Search field at datatable view and summary view -->
	<v-text-field
		v-model="searchValue"
		label="Search"
		single-line
		hide-details
		clearable
		class="ml-4"
	>
	</v-text-field>
	<v-spacer></v-spacer>
	<v-btn icon @click="logOut"><v-icon>mdi-logout</v-icon></v-btn>
	<v-toolbar-title>Digital Menu</v-toolbar-title>
</v-app-bar>
</template>

<script lang="ts">
import axios from 'axios';
import { Component, Vue } from 'vue-property-decorator';
import { logout } from '../auth';


@Component
export default class AppBar extends Vue {

	/* ############################## Data ############################## */

	/* ############################## Computed ############################## */



	/* ############################## Methods ############################## */
	/**
	 * Load file from url which contains current dishes
	 */
	public async loadFile(): Promise<void> {
		const url = "https://localhost:5001/dish";
		try {

			const response = await axios.get(url);
			const dishes = response.data;
			console.log(dishes);

			if (dishes === undefined) {
				this.$notify('LOW', {
					text: 'Online file did not contain any data',
					color: 'error',
				});
				return;
			}

		} catch (error) {
			console.log(error);
			this.$notify('LOW', {
				text: `Error downloading file ${url}`,
				color: 'error',
			});
		}
	}

	/**
	 * Performs logout operation in app by logout from Okta
	 */
	public async logOut(): Promise<void> {
		try {
			logout();
		} catch (error) {
			console.log(error);
		}
	}


	public mounted(): void {
	}
}
</script>

<style lang="less">
.v-divider--vertical {
	align-self: auto !important; // Fix for the divider
	border-color: rgba(255, 255, 255, 0.4) !important;
}

.noDecorate {
	text-decoration: none !important;
}
</style>