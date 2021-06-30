<script>
  import { onMount } from "svelte";
  import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

  let connection = new HubConnectionBuilder()
    .withUrl("https://localhost:6001/hubs/testHub")
    .withAutomaticReconnect()
    .build();

  connection.on("BroadcastMessage", data => {
    game = data.message;
    localGame.push(data.message);
    console.log(localGame);
  });
  
connection.on("Broadcast", data => {
    console.log(data);
  });


  connection.on("SystemHeartbeatEvent", (data, b) => {
    // game = data.message;
    // localGame.push(data.message);
    console.log(data);
    console.log(b);
    console.log(arguments)
  });

  
  async function start() {
    try {
        await connection.start();
        connection.invoke("Broadcast", "user", "message");
        console.log(connection.state);
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(connection.state);
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};




  let localGame = [];
  export let name;
  export let game;

  let celebrations = [];
  let nextUp;

  let messages = [];

  onMount(async () => {
    await start();
    const res = await fetch(`https://localhost:6001/api/celebrations`);
    const dasd = await res.json();

    celebrations = dasd;
    nextUp = celebrations.shift();
    name = dasd[0].title;
  });
</script>

<style>
  @import url('https://fonts.googleapis.com/css2?family=Bangers&family=Open+Sans&display=swap');

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

  h1,h2 {
    font-family: 'Bangers';
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
