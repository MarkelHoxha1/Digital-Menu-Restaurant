module.exports = {
	preset: '@vue/cli-plugin-unit-jest/presets/typescript',
	moduleFileExtensions: [
		'js',
		'jsx',
		'json',
		'vue',
		'ts',
		'tsx'
	],
	transform: {
		'^.+\\.jsx?$': 'babel-jest',
		'^.+\\.vue$': 'vue-jest',
		'.+\\.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$': 'jest-transform-stub',
		'^.+\\.tsx?$': 'ts-jest'

	},
	verbose: true,
	moduleNameMapper: {
		'^@/(.*)$': '<rootDir>/src/$1'
	},
	snapshotSerializers: [
		'jest-serializer-vue'
	],
	testMatch: [
		'**/tests/unit/**/*.spec.(js|jsx|ts|tsx)|**/__tests__/*.(js|jsx|ts|tsx)'
	],
	testURL: 'http://localhost/',
	globals: {
		'ts-jest': {
			babelConfig: true,
			isolatedModules: true
		}
	},
	reporters: [
		'default',
		['jest-junit', { outputDirectory: 'junit' }]
	]
};
