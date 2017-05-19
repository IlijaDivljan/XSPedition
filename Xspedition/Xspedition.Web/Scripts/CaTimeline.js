var CaTimeline = (function() {
    var that = {},
        timelineDateRanges,
        timelineDateFields = [],
        caSingleViewTimeline,
        pieChart,
        singleProcessViewTimeline;

    that.caId = 0;
    that.processesModels = [];

    that.loadCaTimeline = function(caId) {
        $.ajax({
            type: 'GET',
            url: root + "Xspedition/GetCaTimelineModel?caId=" + caId,
            cache: false,
            success: function (result) {
                if (result.Error) {
                    return;
                }

                _init(result);
                viewModel.selectedCaId(caId);
            }
        });
    }

    that.replaceSingleProcess = function (processModel) {
        var process;

        if (CaTimeline.caId !== processModel.CaId) {
            return;
        }

        process = ko.utils.arrayFirst(viewModel.processes(), function (proc) {
            return proc.id === processModel.id;
        });

        CaTimeline.processesModels = CaTimeline.processesModels.filter(function (pr) { return pr.id !== processModel.id });
        CaTimeline.processesModels.push(processModel);
        
        if (process) {
            viewModel.processes.replace(process,
                new Process(processModel.id,
                    processModel.title,
                    processModel.targetDateItems,
                    processModel.criticalDateItems,
                    processModel.lateDateItems,
                    processModel.missingItems,
                    processModel.processActionName,
                    processModel.totalItemCount,
                    processModel.processedItemCount,
                    processModel.processPercentage));
        }
    }

    var _init = function(model) {
        that.caId = model.CaId;
        that.processesModels = model.CaProcessViewModels;
        timelineDateRanges = model.TimelineRegions;
        timelineDateFields = model.TimelineDateFields;

        _initializeSingleCaViewTimeline();
        _initializeSingleCaViewProcesses();
    }

    var _initializeSingleCaViewTimeline = function() {
        var container = document.getElementById("timeline"),
            data = [],
            options = {
                start: timelineDateRanges.ScrubbingStart,
                end: timelineDateRanges.PaymentCritical,
                editable: false
            };

        if (caSingleViewTimeline) {
            caSingleViewTimeline = null;
            $("#timeline").html("");
        }

        data = data.concat(_getTimelineItems());
        data = data.concat(_getTimelineRegions());

        caSingleViewTimeline = new vis.Timeline(container, new vis.DataSet(data), options);
    }

    var _initializeSingleCaViewProcesses = function () {
        var processModel,
            process;
        
        for (processModel in CaTimeline.processesModels) {
            if (CaTimeline.processesModels.hasOwnProperty(processModel)) {
                
                process = ko.utils.arrayFirst(viewModel.processes(), function (proc) {
                    return proc.id === CaTimeline.processesModels[processModel].id;
                });

                if (process) {
                    viewModel.processes.replace(process,
                        new Process(CaTimeline.processesModels[processModel].id,
                            CaTimeline.processesModels[processModel].title,
                            CaTimeline.processesModels[processModel].targetDateItems,
                            CaTimeline.processesModels[processModel].criticalDateItems,
                            CaTimeline.processesModels[processModel].lateDateItems,
                            CaTimeline.processesModels[processModel].missingItems,
                            CaTimeline.processesModels[processModel].processActionName,
                            CaTimeline.processesModels[processModel].totalItemCount,
                            CaTimeline.processesModels[processModel].processedItemCount,
                            CaTimeline.processesModels[processModel].processPercentage));
                }
            }
        }
    }

    that.displaySingleProcessView = function (processId) {
        _initSingleProcessTimelineView(processId);
        viewModel.showSingleProcess(true);
        //_initSingleViewPieChart(processId);
    }

    var _initSingleViewPieChart = function (processId) {
        var processModel = CaTimeline.processesModels.filter(function (pr) { return pr.id === processId })[0];

        if (!processModel) {
            return;
        }

        if (pieChart) {
            pieChart = null;
            $("#single-process-pie-chart").html("");
        }

        pieChart = Highcharts.chart('single-process-pie-chart', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'CA (' + CaTimeline.caId + ') ' + getProcessName(processModel.processType) + ' Process Summary'//
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.2f} %'
                    }
                }
            },
            series: [{
                data: [
                    { name: 'Target', y: (processModel.targetDateItems.length / (processModel.totalItemCount === 0 ? 1 : processModel.totalItemCount)), sliced: true, selected: true },//56.33
                    { name: 'Critical', y: (processModel.criticalDateItems.length / (processModel.totalItemCount === 0 ? 1 : processModel.totalItemCount)) },//24.03
                    { name: 'Late', y: (processModel.lateDateItems.length / (processModel.totalItemCount === 0 ? 1 : processModel.totalItemCount)) },
                    { name: 'Missing', y: (processModel.missingItems.length / (processModel.totalItemCount === 0 ? 1 : processModel.totalItemCount)) }
                ], colors: ['#8dc63f', '#ffc845', '#990000', '#222222']
            }]
        });
    }

    var _initSingleProcessTimelineView = function(processId) {
        var processModel = CaTimeline.processesModels.filter(function (pr) { return pr.id === processId })[0],
            container = document.getElementById("single-process-timeline"),
            data = [],
            options = {};

        if (!processModel) {
            return;
        }

        viewModel.processName(getProcessName(processModel.processType));

        if (singleProcessViewTimeline) {
            singleProcessViewTimeline = null;
            $("#single-process-timeline").html("");
        }

        data = data.concat(_getSingleProcessViewTimelineItems(processModel.TimelineItems));
        options = _getSingleTimelineRegionOptions(processId);

        var newOptions = {
            editable: false
        };

        if (options.start && options.end) {
            data.push({
                id: "single-process-timeline-chart-" + processId,
                content: getProcessName(processModel.processType),
                start: options.start,
                end: options.end,
                className: "single-timeline-" + processId + "-region",
                type: 'background'
            });

            var regionStart = new Date(options.start);
            regionStart.setDate(regionStart.getDate() - 2);

            var regionEnd = new Date(options.end);
            regionEnd.setDate(regionEnd.getDate() + 2);

            newOptions.start = regionStart;
            newOptions.end = regionEnd;

        }
        
        singleProcessViewTimeline = new vis.Timeline(container, new vis.DataSet(data), newOptions);

        if (newOptions.start) {
            singleProcessViewTimeline.addCustomTime(options.targetDate);
        }
    }

    var _getSingleTimelineRegionOptions = function(processId) {
        var options = {
            editable: false
        };
        
        switch(processId) {
            case "scrub":
                options.start = new Date(parseInt(timelineDateRanges.ScrubbingStart.replace('/Date(', '')));
                options.end = new Date(parseInt(timelineDateRanges.ScrubbingCritical.replace('/Date(', '')));
                options.targetDate = timelineDateRanges.ScrubbingTarget;
                break;
            case "notif":
                options.start = new Date(parseInt(timelineDateRanges.NotificationStart.replace('/Date(', '')));
                options.end = new Date(parseInt(timelineDateRanges.NotificationCritical.replace('/Date(', '')));
                options.targetDate = timelineDateRanges.NotificationTarget;
                break;
            case "respo":
                if (timelineDateRanges.ResponseStart && timelineDateRanges.ResponseCritical) {
                    options.start = new Date(parseInt(timelineDateRanges.ResponseStart.replace('/Date(', '')));
                    options.end = new Date(parseInt(timelineDateRanges.ResponseCritical.replace('/Date(', '')));
                    options.targetDate = timelineDateRanges.ResponseTarget;
                }
                break;
            case "instr":
                if (timelineDateRanges.InstructionStart && timelineDateRanges.InstructionCritical) {
                    options.start = new Date(parseInt(timelineDateRanges.InstructionStart.replace('/Date(', '')));
                    options.end = new Date(parseInt(timelineDateRanges.InstructionCritical.replace('/Date(', '')));
                    options.targetDate = timelineDateRanges.InstructionTarget;
                }
                break;
            case "payme":
                options.start = new Date(parseInt(timelineDateRanges.PaymentStart.replace('/Date(', '')));
                options.end = new Date(parseInt(timelineDateRanges.PaymentCritical.replace('/Date(', '')));
                options.targetDate = timelineDateRanges.PaymentTarget;
                break;
        }

        return options;
    }

    var _getTimelineRegions = function() {
        var regions = [];
        //new Date(parseInt(jsonDateString.replace('/Date(', '')))
        regions.push({
            id: "scrubT",
            content: "Scrubbing",
            start: timelineDateRanges.ScrubbingStart,
            end: timelineDateRanges.ScrubbingCritical,
            className: "single-timeline-scrub-region",
            type: 'background'
        });

        regions.push({
            id: "notifyT",
            content: "Notifications",
            start: timelineDateRanges.NotificationStart,
            end: timelineDateRanges.NotificationCritical,
            className: "single-timeline-notif-region",
            type: 'background'
        });

        if (timelineDateRanges.ResponseStart) {
            regions.push({
                id: "responseT",
                content: "Responses",
                start: timelineDateRanges.ResponseStart,
                end: timelineDateRanges.ResponseCritical,
                className: "single-timeline-respo-region",
                type: 'background'
            });
        }

        if (timelineDateRanges.InstructionStart) {
            regions.push({
                id: "instructionT",
                content: "Instructions",
                start: timelineDateRanges.InstructionStart,
                end: timelineDateRanges.InstructionCritical,
                className: "single-timeline-instr-region",
                type: 'background'
            });
        }

        regions.push({
            id: "paymentT",
            content: "Payments",
            start: timelineDateRanges.PaymentStart,
            end: timelineDateRanges.PaymentCritical,
            className: "single-timeline-payme-region",
            type: 'background'
        });

        return regions;
    }

    var _getTimelineItems = function() {
        var timelineDates = [],
            idIncrement = 1;

        for(var fld in timelineDateFields)
        {
            if (timelineDateFields.hasOwnProperty(fld)) {
                timelineDates.push({
                    id: "timelineFld" + (idIncrement++),
                    content: timelineDateFields[fld].Content,
                    start: timelineDateFields[fld].Date,
                    className: "single-timeline-field"
                });
            }
        }

        return timelineDates;
    }

    var _getSingleProcessViewTimelineItems = function (timelineItems) {
        var timelineDates = [],
            idIncrement = 1;

        for (var fld in timelineItems) {
            if (timelineItems.hasOwnProperty(fld)) {
                timelineDates.push({
                    id: "timelineFld" + (idIncrement++),
                    content: timelineItems[fld].Content,
                    start: timelineItems[fld].Date,
                    className: "single-timeline-field single-timeline-field-" + timelineItems[fld].ProcessedDateCategory
                });
            }
        }

        return timelineDates;
    }

    return that;
}());