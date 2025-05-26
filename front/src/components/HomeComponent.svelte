<script lang="ts">
    import { DataService } from "../services/data.service";
    import { onDestroy, onMount } from "svelte";
    import type { Data } from "../entities/data";
    import { SignalRService } from "../services/signalr.service";
    import AddModal from "./AddModal.svelte";
    import chartjs from 'chart.js/auto';
	import { DatePicker } from '@svelte-plugins/datepicker';
	import { format } from 'date-fns';

    let data = $state([] as Data[]);
    let startDate = $state(new Date(Date.now() - 30 * 24 * 60 * 60 * 1000)); // 7 days ago
    let endDate = $state(new Date(Date.now())); // today
    // $inspect("inspect data", data);
    // $inspect("inspect startDate", startDate);
    // $inspect("inspect endDate", endDate);
    let chartY: number[] = [];
    let chartX: string[] = [];
    let ctx;
    let chartCanvas: any;
    let chart: any;
    let showDataModal = $state(false);
    let showDateRangeModal = $state(false);
    let isOpen = $state(false);
    const toggleDatePicker = () => (isOpen = !isOpen);
    const dateFormat = 'MMM d, yyyy';
        const formatDate = (date: Date) => {
		return date && format(date, dateFormat) || '';
	};
	let formattedstartDate = $state(formatDate(startDate));
	let formattedendDate = $state(formatDate(endDate));

    const setupChart = () => {
        if (chartCanvas) {
            let filteredData = data.filter(item => {
                let createdAt = new Date(item.createdAt);
                return createdAt >= new Date(startDate) && createdAt <= new Date(endDate);
            });
            ctx = chartCanvas.getContext('2d');
            chartY = filteredData.map((item) => item.value);
            chartX = filteredData.map((item) => item.createdAt.toString().split("T")[0]);
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
        setupChart();
    });
    
    onMount(async () => {
        let stockService = new DataService();
        data = await stockService.getAll();
        setupChart();
    });

    onDestroy(() => {
        SignalRService.off("messageReceived", () => {});
    });

    let setupStartDate = (value: any) => {
		startDate = new Date(value);
		startDate.setHours(0, 0, 0, 0);
		formattedstartDate = formatDate(startDate);
	};

	let setupEndDate = (value: any) => {
		endDate = new Date(value);
		endDate.setHours(23, 59, 59, 999);
		formattedendDate = formatDate(endDate);
	};

    const onDateChange = (args: any) => {
        setupStartDate(args.startDate);
        setupEndDate(args.endDate);
    };

</script>

<AddModal bind:showDataModal bind:data/>
<div class="inline-block mb-4">
<button onclick={() => {showDataModal = !showDataModal}} type="button" class="cursor-pointer text-white bg-gray-800 hover:bg-gray-700 font-medium text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-600 dark:hover:bg-gray-700 dark:focus:ring-blue-800">
    Add data
</button>
</div>
<div class="date-filter inline-block mb-4 cursor-pointer">
<DatePicker bind:isOpen bind:startDate bind:endDate isRange showPresets isMultipane {onDateChange} >
    <div class="date-field" onclick={toggleDatePicker} class:open={isOpen}>
      <i class="icon-calendar" />
      <div class="date">
        {#if startDate}
          {formattedstartDate} - {formattedendDate}
        {:else}
          Pick a date
        {/if}
      </div>
    </div>
  </DatePicker>
</div>
<div class="w-[90%] m-auto">
    <canvas bind:this={chartCanvas} id="myChart"></canvas>
</div>

<style>
    .date-field {
        align-items: center;
        background-color: #fff;
        border-bottom: 1px solid #e8e9ea;
        display: inline-flex;
        gap: 8px;
        min-width: 100px;
        padding: 16px;
    }

    .date-field.open {
        border-bottom: 1px solid #0087ff;
    }

    .date-field .icon-calendar {
        background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAABYlAAAWJQFJUiTwAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAEmSURBVHgB7ZcPzcIwEMUfXz4BSCgKwAGgACRMAg6YBBxsOMABOAAHFAXgAK5Z2Y6lHbfQ8SfpL3lZaY/1rb01N+BHUKSMNBfEJjZWISA56Uo6C2KvVpkgFn9oRx9vICFtUT1JKO3tvRtZdjBxXQs+YY+1FenIfuesPUGVVLzfRWKvmrSzbbN19wS+kAb2+sCEuUxrYzkbe4YvCVM2Vr5NPAkVa+van7Wn38U95uTpN5TJ/A8ZKemAakmbmJJGpI0gVmwA0huieFItjG19DgTHtwIZhCfZq3ztCuzQYh+FKBSvusjAGs8PnLYkLgMf34JoIBqIBqKBaIAb0Kw9RlhMCTbzzPWAqYq7LsuPaGDUsYmznaOk5zChUJTNQ4TFVMkrOL4HPsoNn26PxROHCggAAAAASUVORK5CYII=) no-repeat center center;
        background-size: 14px 14px;
        height: 14px;
        width: 14px;
    }
</style>