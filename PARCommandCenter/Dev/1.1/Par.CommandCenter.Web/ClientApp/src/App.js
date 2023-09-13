import React from 'react';

import "@fortawesome/fontawesome-free/css/all.min.css";
import "bootstrap/dist/css/bootstrap.min.css";

import "./assets/scss/light-bootstrap-dashboard-pro-react.scss?v=2.0.0";

import './App.scss';


import Layout from './components/_Application/Layout';

import { BrowserRouter, Route, Switch, Redirect } from 'react-router-dom';

import AppStateProvider from './state';

function App() {    
  
    return (
        
        <AppStateProvider>
            <BrowserRouter>
                <Switch>
                    <Route path="/dashboard" render={(props) => <Layout {...props} />} />
                    <Route path="/dashboard/vpns" render={(props) => <Layout {...props} />} />
                    <Route path="/dashboard/interfaces" render={(props) => <Layout {...props} />} />
                    <Route path="/dashboard/routers" render={(props) => <Layout {...props} />} />
                    <Route path="/dashboard/controllers" render={(props) => <Layout {...props} />} />
                    <Route path="/dashboard/scales" render={(props) => <Layout {...props} />} />

                    <Route path="/map" render={(props) => <Layout {...props} />} />
                    <Route path="/setup" render={(props) => <Layout {...props} />} />
                    <Route path="/router" render={(props) => <Layout {...props} />} />
                    <Route path="/items" render={(props) => <Layout {...props} />} />
                    <Route path="/controller" render={(props) => <Layout {...props} />} />
                    <Route path="/scale" render={(props) => <Layout {...props} />} />
                    <Route path="/facilities" render={(props) => <Layout {...props} />} />
                    <Route path="/setting" render={(props) => <Layout {...props} />} />
                    <Route path="/auditLog" render={(props) => <Layout {...props} />} />
                  
                    <Route path="/tenants" render={(props) => <Layout {...props} />} />
                    <Route path="/tenants/summary" render={(props) => <Layout {...props} />} />

                    <Route path="/cloud-routers" render={(props) => <Layout {...props} />} />
                    <Route path="/403-forbidden" render={(props) => <Layout {...props} logoOnly={true} />} />
                    <Redirect from="/index" to="/dashboard" />

                    <Route path="/products" render={(props) => <Layout {...props} />} />

                    {/* Integrations routes */}
                    <Route path="/hl7CloudServers" render={(props) => <Layout {...props} />} />
                    <Route path="/Queues" render={(props) => <Layout {...props} />} />
                    <Route path="/QueuesHistory" render={(props) => <Layout {...props} />} />
                </Switch>
            </BrowserRouter>
        </AppStateProvider>
    );
}

export default App;
