<script lang="ts">
    import { DataService } from "../services/data.service";
    import { onDestroy, onMount } from "svelte";
    import type { Data } from "../entities/data";
    import { SignalRService } from "../services/signalr.service";
    import AddModal from "./AddModal.svelte";
    import chartjs from 'chart.js/auto';
    let data = $state([] as Data[]);
    $inspect("inspect data", data);
    let chartY: number[] = [];
    let chartX: string[] = [];
    let ctx;
    let chartCanvas: any;
    let chart: any;
    let showModal = $state(false);

    const setupChart = () => {
        if (chartCanvas) {
            ctx = chartCanvas.getContext('2d');
            chartY = data.map((item) => item.value);
            chartX = data.map((item) => item.createdAt.toString().split("T")[0]);
            chart?.destroy();
            chart = new chartjs(ctx, {
                type: 'line',
                data: {
                    labels: chartX,
                    datasets: [{
                        label: 'Data',
                        backgroundColor: 'rgb(255, 99, 132)',
                        borderColor: 'rgb(255, 99, 132)',
                        data: chartY
                    }]
                }
            });
        }
    };

    $effect(() => {
        console.log("data changed", data);
        setupChart();
    });
    
    onMount(async () => {
        let stockService = new DataService();
        data = await stockService.getAll();
        console.log("data", data);
        setupChart();
    });

    onDestroy(() => {
        SignalRService.off("messageReceived", () => {});
    });

</script>

<style>
</style>

<AddModal bind:showModal bind:data/>
<button onclick={() => {showModal = !showModal}} type="button" class="cursor-pointer text-white bg-gray-800 hover:bg-gray-700 font-medium text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-600 dark:hover:bg-gray-700 dark:focus:ring-blue-800">
    Add data
</button>
<div class="w-[90%] m-auto">
    <canvas bind:this={chartCanvas} id="myChart"></canvas>
</div>