<script lang="ts">
    import { onMount } from 'svelte';

    type ShoppingItem = {
        id: number;
        item: string;
        amount: number;
    }

    let shoppingItems = $state<ShoppingItem[]>([]);

    onMount(async () => {
        try{
            const response = await fetch(
                'https://localhost:7004/api/shoppinglist'
            );

            if (!response.ok){
                throw new Error('Could not get the shoppinglist');
            }

            shoppingItems = await response.json();

            console.log(shoppingItems);
        }
        catch{
            console.log('Could not load') 
        }
    })
</script>

<main>
    <h1>ShoppingList</h1>

    <div>
        {#each shoppingItems as shoppingItem (shoppingItem.id)}
            <p>{shoppingItem.item} - Amount: {shoppingItem.amount}</p>
        {/each}
    </div>
   
</main>

<style>
    :global(body){
        background-color: darkolivegreen;
    }
</style>


