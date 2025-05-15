<script lang="ts">
    import {DataService} from "../services/data.service";
    import {Data} from "../entities/data";
	let { showModal = $bindable(), data = $bindable() } = $props();

    let dialog: any = $state(); // HTMLDialogElement
    let newdata: any = $state(42);

	$effect(() => {
		if (showModal) dialog.showModal();
	});

    let submitData = async () => {
        let dataService = new DataService();
        let createdData = await dataService.create(new Data(0, newdata, new Date()));
        data.push(createdData);
        console.log("data", data);
        dialog.close();
    };
</script>

<dialog
	bind:this={dialog}
	onclose={() => (showModal = false)}
	onclick={(e) => { if (e.target === dialog) dialog.close(); }}

>
	<div class="modal-container">
        
		<!-- svelte-ignore a11y_autofocus -->
         <div class="modal-body">
		    <h2>Add data</h2>
            <label for="data" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Data</label>
            <input
            pattern="[0-9]+(.[0-9])?"
            bind:value={newdata}
            type="text"
            id="data"
            class="bg-gray-50 text-gray-900 text-sm block w-full p-2.5 dark:bg-gray-700 dark:placeholder-gray-400 dark:text-white" placeholder="value" required />
          </div>
         <div class="modal-footer">
            <button onclick={submitData} type="button" class="cursor-pointer text-white bg-gray-800 hover:bg-gray-700 font-medium text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-600 dark:hover:bg-gray-700 dark:focus:ring-blue-800">
                Submit
            </button>
            <button onclick={() => dialog.close()} type="button" class="cursor-pointer text-white bg-gray-800 hover:bg-gray-700 font-medium text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-600 dark:hover:bg-gray-700 dark:focus:ring-blue-800">
                Close
            </button>
         </div>

	</div>
</dialog>

<style>
	dialog {
		width: 400px;
        height: 200px;
		border: none;
		padding: 0;
        margin: auto;
	}
	dialog::backdrop {
		background: rgba(0, 0, 0, 0.3);
	}
	dialog > div {
		padding: 1em;
	}
	dialog[open] {
		animation: zoom 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
	}
	@keyframes zoom {
		from {
			transform: scale(0.95);
		}
		to {
			transform: scale(1);
		}
	}
	dialog[open]::backdrop {
		animation: fade 0.2s ease-out;
	}
	@keyframes fade {
		from {
			opacity: 0;
		}
		to {
			opacity: 1;
		}
	}
    .modal-container {
        height: 100%;
    }
    .modal-body {
        height: 80%;
    }
    .modal-footer {
        height: 10%;
    }
</style>