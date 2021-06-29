import App from './App.svelte';

const app = new App({
    target: document.body,
    props: {
        name: 'Master',
        game: 'blaster the game'
    }
});

export default app;