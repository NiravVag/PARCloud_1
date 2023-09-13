import React, { useEffect, useState } from 'react';

import { useAppState } from "./../../state";

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Collapse,
    Form,
    InputGroup,
    Navbar,
    Nav,
    OverlayTrigger,
    Table,
    Tooltip,
    Container,
    Row,
    Col,
    Modal,
    Tab
} from "react-bootstrap";



import "./SettingPage.scss";

import SelectTenantsPage from "./SelectTenants/SelectTenantsPage";

import NotificationSettingPage from "./Notification/NotificationSettingPage";


function SettingPage(props) {

    return (
        <>
            <Container fluid id="settingPage">                
                <Card className={`bg-white border-0`}>                   
                    <Card.Body>
                        <Tab.Container
                            id="icons-tabs-example"
                            defaultActiveKey="tenants-setting"
                            mountOnEnter={true}
                        >
                            <Nav role="tablist" variant="tabs">
                                <Nav.Item>
                                    <Nav.Link eventKey="tenants-setting">
                                        <i className="menu-icon fas fa-hospital-user"></i>
                                        <span className="pl-1 menu-name">Tenants</span>
                                    </Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link eventKey="account-icons" >
                                        <i className="menu-icon fas fa-bell"></i>
                                        <span className="pl-1 menu-name">Notifications</span>
                                    </Nav.Link>
                                </Nav.Item>                           
                            </Nav>
                            <Tab.Content>
                                <Tab.Pane eventKey="tenants-setting">
                                    <SelectTenantsPage />
                                </Tab.Pane>
                                <Tab.Pane eventKey="account-icons">
                                    <NotificationSettingPage />
                                </Tab.Pane>                               
                            </Tab.Content>
                        </Tab.Container>
                    </Card.Body>                   
                </Card>
            </Container>
        </>
    );
}




export default SettingPage;






