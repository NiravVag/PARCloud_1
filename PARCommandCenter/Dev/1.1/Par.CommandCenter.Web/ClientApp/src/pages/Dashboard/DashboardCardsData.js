const DashboardCardsData = [
    {
        indexNumber: 0,
        headerName: "Hosting Info",
        headerIcon: "fas fa-exclamation-triangle text-danger",
        icon: " fa-medium-size fas fa-server",
        isSpecialType: true,
        cardComponentName: "AppHostingInfoCard",
        cardDataUrl: "api/Application/HostingInfo",
        isDevOnly: true,
    },
    {      
        indexNumber: 1,
        headerName: "VPNs",
        headerIcon: "fas fa-exclamation-triangle text-danger",
        icon: " fa-medium-size fas fa-wifi",

        cardTitleDanger: "Critical Outages",
        cardTitleWarning: "Configuration Updates",
        cardTitleActive: "VPNs Online",

        cardCountsUrl: "api/HealthCheck/VPNs",
        errorDataUrl: "api/HealthCheck/VPNs",
        warningsDataUrl: "api/HealthCheck/VPNs",
        activeDataUrl: "api/HealthCheck/VPNs",
    },
    {
        indexNumber: 2,
        headerName: "Interfaces",
        headerIcon: "fas fa-exclamation-triangle text-danger",
        icon: " fa-medium-size fas fa-desktop",

        cardTitleDanger: "Critical Outages",
        cardTitleWarning: "Configuration Updates",
        cardTitleActive: "Active Interfaces",

        cardCountsUrl: "api/HealthCheck/interfaces",
        errorDataUrl: "api/HealthCheck/interfaces",
        warningsDataUrl: "api/HealthCheck/interfaces",
        activeDataUrl: "api/HealthCheck/interfaces",
        ftpServerStatusUrl: "api/HealthCheck/FTPServer",
        theme: "ftpServer"
    },
    {
        indexNumber: 3,
        headerName: "Routers",
        headerIcon: "fas fa-exclamation-triangle text-danger",
        icon: " fa-medium-size fas fa-satellite-dish",

        cardTitleDanger: "Critical Outages",
        cardTitleWarning: "Configuration Updates",
        cardTitleActive: "Routers Online",

        cardCountsUrl: "api/HealthCheck/routers",
        errorDataUrl: "api/HealthCheck/routers",
        warningsDataUrl: "api/HealthCheck/routers",
        activeDataUrl: "api/HealthCheck/routers",
    },
    {
        indexNumber: 4,
        headerName: "Controllers",
        headerIcon: "fas fa-exclamation-triangle text-danger",
        icon: " fa-medium-size fas fa-broadcast-tower",

        cardTitleDanger: "Controller Outages",
        cardTitleWarning: "Configuration Updates",
        cardTitleActive: "Controllers Online",

        cardCountsUrl: "api/HealthCheck/controllers",
        errorDataUrl: "api/HealthCheck/controllers",
        warningsDataUrl: "api/HealthCheck/controllers",
        activeDataUrl: "api/HealthCheck/controllers",
    },
    {
        indexNumber: 5,
        headerName: "Scales",
        headerIcon: "fas fa-exclamation-triangle text-danger",
        icon: " fa-medium-size fas fa-server",

        cardTitleDanger: [
            {
                indexNumber: 0,
                title: "Scale Outages",
                url: "api/HealthCheck/scales",
                countProp: "unHealthyCount",
                filter: "errors",
            },
            {
                indexNumber: 1,
                title: "Scales Needs Calibration",
                url: "api/HealthCheck/scales",
                countProp: "scalesMissingCalibrationCount",
                filter: "needCalibration",
            }
        ],
        cardTitleWarning: "Configuration Updates",
        cardTitleActive: "Scales Online",

        cardCountsUrl: "api/HealthCheck/scales",
        errorDataUrl: "api/HealthCheck/scales",
        warningsDataUrl: "api/HealthCheck/scales",
        activeDataUrl: "api/HealthCheck/scales",
    },
];

export default DashboardCardsData;