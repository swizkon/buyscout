<script>
  import { onMount } from "svelte";
  import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";

  let connection = new HubConnectionBuilder()
    .withUrl("https://localhost:6001/signalr")
    .build();

  connection
    .start()
    .then(() => console.log("Connection started!"))
    .catch(err => console.log("Error while establishing connection :("));
  connection.on("BroadcastMessage", data => {
    game = data.message;
    localGame.push(data.message);
    console.log(localGame);
  });
  let localGame = [];
  export let name;
  export let game;

  let celebrations = [];
  let nextUp;

  let messages = [];

  onMount(async () => {
    const res = await fetch(`https://localhost:6001/api/celebrations`);
    const dasd = await res.json();

    celebrations = dasd;
    nextUp = celebrations.shift();
    name = dasd[0].title;
  });
</script>

<style>
  main {
    text-align: center;
    padding: 1em;
    max-width: 250px;
    margin: 0 auto;
  }

  h1 {
    color: #369;
    text-transform: uppercase;
    font-size: 4em;
    font-weight: 100;
  }

  @media (min-width: 640px) {
    main {
      max-width: none;
    }
  }
</style>

<main>

  <div class="jumbotron">
    <h2>Allå, allå</h2>
    {#if nextUp != null}
      <h1 id="next-title">{nextUp.title}</h1>
      <h2>{nextUp.description}</h2>
      <h3 id="num-days">{nextUp.nextOccurrence}</h3>
    {:else}
      <p>Loading...</p>
      <h1 id="next-title">Loading...</h1>
    {/if}
  </div>

  <h3>Future:</h3>

  <div>
    <ul>
      {#each celebrations as celebration}
        <li>
          {celebration.title} {celebration.description}
          {celebration.nextOccurrence}
        </li>
      {/each}
    </ul>
  </div>

  <p style="display:none;">
    Visit the
    <a href="https://svelte.dev/tutorial">Svelte tutorial</a>
    to learn how to build Svelte apps.
  </p>
</main>
