import React, { useEffect, useState } from 'react';

import {
    Badge,
    Button,
    ButtonGroup,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup,
    Modal,
    FormControl,
    Spinner,
    Tab
} from "react-bootstrap";

import RestartCloudRouter from "./../../../Common/RestartCloudRouter";
import PcRouterConfiguration from "./PcRouterConfiguration";

import "./RouterUtilityPanel.scss";

const RouterUtilityPanel = (props) => {

    if (props.routerType == "Cloud Router On-PC") {
        return (
            <Container fluid>
                <Row>
                    <Col md="12">
                        <Tab.Container
                            id="icons-tabs-example"
                            defaultActiveKey="restart-router"
                            mountOnEnter={true}
                        >
                            <Nav role="tablist" variant="tabs">
                                <Nav.Item>
                                    <Nav.Link eventKey="restart-router">
                                        <i className="menu-icon fas fa-refresh"></i>
                                        <span className="pl-1 menu-name">Restart</span>
                                    </Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link eventKey="config-router" >
                                        <i className="menu-icon fas fa-cog"></i>
                                        <span className="pl-1 menu-name">PC Configuration</span>
                                    </Nav.Link>
                                </Nav.Item>
                            </Nav>
                            <Tab.Content>
                                <Tab.Pane eventKey="restart-router">
                                    <RestartCloudRouter routerId={props.routerId} routerAddress={props.routerAddress} />
                                </Tab.Pane>
                                <Tab.Pane eventKey="config-router">
                                    <PcRouterConfiguration routerId={props.routerId}></PcRouterConfiguration>
                                </Tab.Pane>
                            </Tab.Content>
                        </Tab.Container>
                    </Col>
                </Row>
            </Container>
        );
    }

    return (
        <Container fluid>
            <Row>
                <Col md="12">
                    <Tab.Container
                        id="icons-tabs-example"
                        defaultActiveKey="restart-router"
                        mountOnEnter={true}
                    >
                        <Nav role="tablist" variant="tabs">
                            <Nav.Item>
                                <Nav.Link eventKey="restart-router">
                                    <i className="menu-icon fas fa-refresh"></i>
                                    <span className="pl-1 menu-name">Restart</span>
                                </Nav.Link>
                            </Nav.Item>
                        </Nav>
                        <Tab.Content>
                            <Tab.Pane eventKey="restart-router">
                                <RestartCloudRouter routerId={props.routerId} routerAddress={props.routerAddress} />
                            </Tab.Pane>
                        </Tab.Content>
                    </Tab.Container>
                </Col>
            </Row>
        </Container>
    );
}

export default RouterUtilityPanel;