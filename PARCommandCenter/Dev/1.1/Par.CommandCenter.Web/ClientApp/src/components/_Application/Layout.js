import React from "react";
//// import { Switch, Route, useLocation, Redirect } from "react-router-dom";
import { Switch, Route} from "react-router-dom";

// core components
import Sidebar from "./Sidebar";
import AppNavbar from "./AppNavbar";
//// import AppFooter from "./AppFooter";



// dynamically create dashboard routes
import routes from "./../../routes";

import { useAppState } from "./../../state";

import ForbiddenPage from "./../../pages/403-Forbidden/ForbiddenPage";

function Layout(props) {
    ////  const [sidebarImage, setSidebarImage] = React.useState(image3);    
    const [sidebarBackground, setSidebarBackground] = React.useState("black");

    const { user, app } = useAppState();     

    const getRoutes = (routes, isUserFirstLogin) => {
        
        return routes.map((prop, key) => {
            if (prop.collapse) {
                return getRoutes(prop.views, isUserFirstLogin);
            }
            if (isUserFirstLogin) {
                if (prop.preComponent) {
                    return (
                        <Route
                            path={prop.path}
                            key={key}
                            component={prop.preComponent}
                        />
                    );
                }
            }
            return (
                <Route
                    path={prop.path}
                    key={key}
                    component={prop.component}
                />
            );
           
        });
    };

    let layoutContent = <div id="loaderDataHome"><div className="row h-50 align-content-end justify-content-center"><span className="loaderPreload loaderLoc"></span></div><div className="text-center col-12 justify-content-center"><h2 className="col"><small>Loading Par Command Data</small></h2></div></div>;

    if (!app.isLoading) {
        if (!user.isAuthorized) {
            layoutContent = (<ForbiddenPage />)
        }
        else {            
            if (props.logoOnly || user.isUserFirstLogin) {
                layoutContent = (
                    
                        <div className="wrapper">
                            <AppNavbar {...props} routes={routes} logoOnly={true} />
                            <div className="main-panel empty-sidebar">
                                <div className="bg-lightgray content p-2">
                                    <Switch>{getRoutes(routes, user.isUserFirstLogin)}</Switch>
                                </div>
                            </div>
                        </div>
                    
                );
            }
            else {
                layoutContent = (
                    <div className="wrapper">
                        <AppNavbar {...props} routes={routes} />
                        <div className="main-panel">
                            <Sidebar
                                routes={routes}
                                background={sidebarBackground}
                            />
                            <div className="bg-lightgray content p-2">
                                <Switch>{getRoutes(routes, user.isUserFirstLogin)}</Switch>
                            </div>
                            <div
                                className="close-layer"
                                onClick={() =>
                                    document.documentElement.classList.toggle("nav-open")
                                }
                            />
                        </div>
                    </div>
                )
            }
        }
    }

    return (
       <>
            {layoutContent}
       </>
    );
}

export default Layout;