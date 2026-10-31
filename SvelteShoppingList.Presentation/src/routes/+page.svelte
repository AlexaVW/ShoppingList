<script lang="ts">
    import { onMount } from 'svelte';

    type ShoppingItem = {
        id: number;
        item: string;
        amount: number;
    }

    let shoppingItems = $state<ShoppingItem[]>([]);

    let newItem = $state({
        item: '',
        amount: 1
    });

    // GET
    onMount(async () => {
        await GetItems();
    });

    async function GetItems(){
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
    }

    async function AddItem(){
        try{
            const response = await fetch(
                'https://localhost:7004/api/shoppinglist',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(newItem)
                }
            );

            if(!response.ok){
                throw new Error('Could not add item');
            }

            const data = await response.json();

            console.log(data);
            
            // Refresh list
            await GetItems();

            newItem = {
                item: '',
                amount: 1
            };

        }
        catch(error){
            console.log('Could not add item', error);
        }
    }

    async function UpdateItem(shoppingItem: ShoppingItem) {
        try{
            const response = await fetch(
                `https://localhost:7004/api/shoppinglist/${shoppingItem.id}`,
                {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(shoppingItem)
                }
            );

            if(!response.ok){
                throw new Error('Could not update item');
            }

            await GetItems();
        }
        catch(error){
            console.log('Could not update item', error);
        }
    }

    async function DeleteItem(id: number) {
        try{
            const response = await fetch(
                `https://localhost:7004/api/shoppinglist/${id}`,
                {
                    method: 'DELETE'
                }
            );

            if(!response.ok){
                throw new Error('Could not delete item');
            }

            await GetItems();
        }
        catch(error){
            console.log('Could not delete item', error);
        }
    }

</script>

<main>
    <h1>ShoppingList</h1>

    <div class="shoppingListBg">
        {#each shoppingItems as shoppingItem (shoppingItem.id)}
            <div class="shoppingItem">
            {shoppingItem.item}

                <input class="amountBar"
                    type="number"
                    min = "1"
                    bind:value={shoppingItem.amount}
                    onchange={() => UpdateItem(shoppingItem)}
                />

            <button onclick={() => DeleteItem(shoppingItem.id)}>
                Delete
            </button>
            </div>
        {/each}
    </div>

    <div>
        <h2>Add new item to shoppinglist</h2>

        <input
            type="text"
            placeholder="Item"
            bind:value={newItem.item}
        />

        <input class="amountBar"
            type="number"
            bind:value={newItem.amount}
        />

        <button onclick={AddItem}>Add</button>
    </div>
   
</main>

<style>
    :global(body){
        background-color: rgb(126, 141, 100);
    }

    main {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .shoppingListBg{
        background-color:#f8e2c8;
        border: 2px solid #613b29;
        border-radius: 10px;
        padding: 40px;
        width: 500px;
        text-align: left;
        
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    .shoppingItem{
        border-bottom: 1px solid #613b29;
        padding-bottom: 0.5rem;
    }

    .amountBar{
        width: 30px;
    }
</style>


