import Vue from 'vue';
export default {
    init: () => {
        Vue.prototype.$notify = (priority, payload) => {
            Vue.prototype.$notifications.$emit('notify', {
                priority, payload,
            });
        };
        Vue.prototype.$notifications = new Vue();
    },
};
//# sourceMappingURL=notifications.js.map