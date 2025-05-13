<script module>
    let count = $state([0,0,0]);
    let der = $derived(() => {
        return count[0] + count[1] + count[2];
    });
</script>

<script lang="ts">
    import { DataService } from "../services/data.service";
    import { onDestroy, onMount } from "svelte";
    import type { Data } from "../entities/data";
    import { SignalRService } from "../services/signalr.service";
    // import CardComponent from "./CardComponent.svelte";
    import chartjs from 'chart.js/auto';
    let props = $props();
    let data = $state([] as Data[]);
    $inspect("data", data);
    let chartY: number[] = [];
    let chartX: string[] = [];
    let ctx;
    let chartCanvas: any;

    onMount(async () => {
        // SignalRService.init();
        // SignalRService.on("messageReceived", (newData: Data[]) => {
        //     console.log('new values', newData);
        //     stocks = newStocks;
        //     filterAndSort(stocks);
        // });
        let stockService = new DataService();
        data = await stockService.getAll();
        console.log("data", data);
        ctx = chartCanvas.getContext('2d');
        chartY = data.map((item) => item.value);
        chartX = data.map((item) => item.createdAt.toString().split("T")[0]);
        var chart = new chartjs(ctx, {
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
    });

    onDestroy(() => {
        SignalRService.off("messageReceived", () => {});
    });

</script>

<style>
</style>

<div class="w-[100%]">
    <canvas bind:this={chartCanvas} id="myChart"></canvas>

</div>