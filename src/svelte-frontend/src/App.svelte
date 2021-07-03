<script>
  import { onMount } from "svelte";
	import { fade, fly } from 'svelte/transition';
	import { flip } from 'svelte/animate';
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
  
connection.on("Broadcast", (data, b, c) => {
    nextUp = {
      title: data,
      description: b,
      nextOccurrence: c
    }
  });

  connection.on("AddItem", (list, title, timestamp) => {
    nextUp = {
      title: title,
      description: list,
      nextOccurrence: timestamp
    }
    celebrations = [nextUp, ...celebrations]; // [nextUp].concat(celebrations);
  });

  const remove = i => {
		celebrations = [...celebrations.slice(0, i), ...celebrations.slice(i + 1)];
	};
  
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
      {#each celebrations as c, i (c)}
        <li animate:flip in:fade out:fly={{x:100}}>
          {c.title} {c.description}
          {c.nextOccurrence}
          <button on:click="{() => remove(i)}">remove</button>
        </li>
      {/each}
    </ul>
  </div>
</main>
