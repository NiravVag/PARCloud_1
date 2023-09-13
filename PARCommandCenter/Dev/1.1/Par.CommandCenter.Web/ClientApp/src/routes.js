import DashboardPage from "./pages/Dashboard/DashboardPage";
import SelectTenantsPage from "./pages/Setting/SelectTenants/SelectTenantsPage";
/*import TenantSetupPage from "./pages/TenantSetup/TenantSetupPage";*/
import CloudRoutersPage from "./pages/CloudRouters/CloudRoutersPage";
import InteractivMapPage from "./pages/InteractiveMap/InteractiveMapPage";
import RouterViewPage from "./pages/TenantSetup/Routers/RouterViewPage";
import ControllerViewPage from "./pages/TenantSetup/Controllers/ControllerViewPage";
import FacilityViewPage from "./pages/TenantSetup/Facilities/FacilityViewPage";
import ScaleViewPage from "./pages/TenantSetup/Scales/ScaleViewPage";
import ItemViewPage from "./pages/TenantSetup/Items/ItemViewPage";
import ForbiddenPage from "./pages/403-Forbidden/ForbiddenPage";

import SettingPage from "./pages/Setting/SettingPage";

import AuditLogPage from "./pages/AuditLog/AuditLogPage";

import HL7CloudServersPage from "./pages/Integrations/HL7CloudServers/HL7CloudServersPage";

import QueuesPage from "./pages/Integrations/Interfaces/Queues/QueuesPage";


import GlobalProductsPage from "./pages/Products/GlobalProductsPage";

import TenantsSummaryPage from "./pages/TenantSetup/Summary/TenantsSummaryPage";

var routes = [
    {
        path: "/setting",
        name: "Setting",
        preComponent: SelectTenantsPage,
        component: SettingPage,
        redirect: true,
    },
    {
        path: "/dashboard",
        name: "Dashboard",
        icon: "menu-icon fas fa-bell",
        preComponent: SelectTenantsPage,
        component: DashboardPage,
    },

    {
        collapse: true,
        state: "tenants", // This value is used as key for the sub-menu state in the Sidebar component
        path: "/setup",
        name: "Tenants",
        icon: "menu-icon fas fa-hospital-user",
        //preComponent: SelectTenantsPage,
        //component: TenantSetupPage,
        views: [
            {
                path: "/tenants/summary",
                name: "Summary",
                /*icon: "menu-icon fas fa-cogs",*/
                preComponent: SelectTenantsPage,
                component: TenantsSummaryPage,
            },
            {
                path: "/facilities",
                name: "Facilities",
                /*icon: "menu-icon fas fa-cogs",*/
                preComponent: SelectTenantsPage,
                component: FacilityViewPage,
            },
            {
                path: "/router",
                name: "Routers",
                /*icon: "menu-icon fas fa-cogs",*/
                preComponent: SelectTenantsPage,
                component: RouterViewPage,
            },
            {
                path: "/controller",
                name: "Controllers",
                /*icon: "menu-icon fas fa-cogs",*/
                preComponent: SelectTenantsPage,
                component: ControllerViewPage,
            },
            {
                path: "/scale",
                name: "Scales",
                /*icon: "menu-icon fas fa-cogs",*/
                preComponent: SelectTenantsPage,
                component: ScaleViewPage,
            },
            {
                path: "/items",
                name: "Items",
                /*icon: "menu-icon fas fa-cogs",*/
                preComponent: SelectTenantsPage,
                component: ItemViewPage,
            },

            //{
            //    path: "/tenants",
            //    name: "Select Tenants",
            //  /*  icon: "menu-icon fas fa-hospital-user",*/
            //    preComponent: <SelectTenantsPage showPageTitle={true} />,
            //    component: SelectTenantsPage,
            //},
           
        ],
    },
    {
        path: "/products",
        name: "Global Products",
        icon: "menu-icon fas fa-list-alt",
        preComponent: SelectTenantsPage,
        component: GlobalProductsPage,
    },
    {
        collapse: true,
        state: "integrations", // This value is used as key for the sub-menu state in the Sidebar component 
        path: "/integrations",
        name: "Integrations",
        icon: "menu-icon fas fa-cloud-upload-alt",      
        views: [
            {
                path: "/Queues",
                name: "Current Queues",
                preComponent: SelectTenantsPage,
                component: QueuesPage,
            },
            //{
            //    path: "/QueuesHistory",
            //    name: "Queues History",
            //    preComponent: SelectTenantsPage,
            //    component: QueuesPage,
            //},
            {
                path: "/hl7CloudServers",
                name: "HL7 Cloud Servers",               
                preComponent: SelectTenantsPage,
                component: HL7CloudServersPage,
            },
        ],
    },    
    {
        path: "/map",
        name: "Interactive Map",
        icon: "menu-icon fas fa-map-marker-alt",
        preComponent: SelectTenantsPage,
        component: InteractivMapPage,
    },
    {
        path: "/auditLog",
        name: "Audit Log",
        icon: "menu-icon fas fa-history",
        preComponent: SelectTenantsPage,
        component: AuditLogPage,
    },
    {
        path: "/403-forbidden",
        name: "403 Forbidden",
        icon: "",
        component: ForbiddenPage,
        redirect: true
    },
];



export default routes;