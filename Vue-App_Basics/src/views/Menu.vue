<template>
	<v-row>
		<v-col>
			<v-row>
				<table-editor></table-editor>
			</v-row>
		</v-col>
	</v-row>
</template>

<script lang="ts">
import TableEditor from '@/components/TableEditor.vue';
import { Component, Vue } from 'vue-property-decorator';

@Component({
	components: {
		TableEditor,
	},
})
export default class Menu extends Vue {

	/* ############################## Data ############################## */


	/* ############################## Computed ############################## */

	/* ############################## Methods ############################## */
	public mounted() {
		// // listens for triggering the navigation drawer
		// this.$root.$on('showNewsViewer', this.showOrHideNewsViewer);
		// // listens for event whiich will open the modal with news versions
		// this.$root.$on('seeVersionsButtonClicked', this.openModalsWithVersions);
	}
}
</script>
<style scoped>
.container{
	padding: 0 20px 0 20px !important;
}
</style>