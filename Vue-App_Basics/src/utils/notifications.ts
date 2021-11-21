import Vue from 'vue';

export type NotificationPriority = 'LOW'| 'HIGH';

interface NotificationPayloadBase {
	text: string;
}

interface NotificationButton {
	color: string;
	callback?: () => void;
	text: string;
}

export interface LowNotificationPayload extends NotificationPayloadBase {
	color: string;
}

export interface HighNotificationPayload extends NotificationPayloadBase {
	title: string;
	persistent: boolean;
	cancel: NotificationButton;
	accept: NotificationButton;
}

export type NotificationPayload = LowNotificationPayload | HighNotificationPayload;

declare module 'vue/types/vue' {
	interface Vue {
		$notifications: Vue;
		$notify: (priority: NotificationPriority, payload: NotificationPayload) => void;
	}
}

export default {
	init: () => {
		Vue.prototype.$notify = (priority: NotificationPriority, payload: NotificationPayload) => {
			Vue.prototype.$notifications.$emit('notify', {
				priority, payload,
			});
		};
		Vue.prototype.$notifications = new Vue();
	},
};
