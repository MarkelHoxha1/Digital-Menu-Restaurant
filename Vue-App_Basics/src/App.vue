<template>
<v-app>
	<app-bar></app-bar>
	<v-content>
		<router-view></router-view>
	</v-content>
</v-app>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import AppBar from '@/components/AppBar.vue';

@Component({
	components: {
		AppBar
	},
})

export default class App extends Vue {
}
</script>

<style lang="less">
// SCENE style buttons
.sc-btn {
	background-color: #e5e5e6 !important;
	box-shadow: none !important;
	font-weight: normal !important;
	color: black !important;
	text-transform: none !important;
}
</style>
<style lang="less">
// ########### Use custom scrollbar #############

::-webkit-scrollbar {
	width: 14px;
	z-index: 10000;
	height: 14px;
}

::-webkit-scrollbar-track {
	background: #FAFAFA; 
}

::-webkit-scrollbar-thumb {
	background: rgba(100,100,100,.5);
}

::-webkit-scrollbar-thumb:hover {
	background: rgba(100,100,100,.8);
}
</style>