import adapter from '@sveltejs/adapter-static';
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte';

const config = {
	preprocess: vitePreprocess(),

	kit: {
		adapter: adapter({
			pages: 'build',
			assets: 'build',
			fallback: 'index.html',
			precompress: false,
			strict: true
		}),
		csp: {
			mode: 'auto',
			directives: {
				'default-src': ["'self'"],
				'script-src': ["'self'", "'unsafe-inline'"], // Unity often needs unsafe-inline
				'connect-src': ["'self'", "https://api.agoraboardgames.com", "https://*.discordsays.com"],
				'img-src': ["'self'", "data:", "blob:"],
				'style-src': ["'self'", "'unsafe-inline'"]
			}
		}
	}
};


export default config;
