import 'babel-polyfill'; // This is only needed  to support IE11. Makes the build heavier so remove once we deprecate it.
import Vue from 'vue';
import App from '@/App.vue';
import router from '@/router';
import vuetify from '@/plugins/vuetify';
import Notifications from '@/utils/notifications';
Notifications.init();
Vue.config.productionTip = false;
new Vue({
    router,
    vuetify,
    render: (h) => h(App),
}).$mount('#app');
//# sourceMappingURL=main.js.map