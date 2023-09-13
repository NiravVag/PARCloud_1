import React, { useState } from 'react';

// react-bootstrap components
import {
    Badge,
    Button,
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
    Tab
} from "react-bootstrap";

import JobQueuePanel from './JobQueue/JobQueuePanel';

import InputDataQueuePanel from './InputDataQueue/InputDataQueuePanel';

import OutputDataQueuePanel from './OutputDataQueue/OutputDataQueuePanel';

import InventoryEventQueuePanel from './InventoryEventQueue/InventoryEventQueuePanel';

import OrderEventQueuePanel from './OrderEventQueue/OrderEventQueuePanel';

const QueuesPanelContainer = (props) => {

    return (
        <>
            <Col md="9" id="QueuesPanel">
                <Card className="bg-white p-2 border-0">
                    <Card.Body className="table-full-width">
                        <Col className="mx-auto pt-2">
                            <Tab.Container
                                id="icons-tabs-example"
                                defaultActiveKey="job-queue"
                                mountOnEnter={true}
                            >
                                <Nav role="tablist" variant="tabs" className="card-header-tabs">
                                    <Nav.Item>
                                        <Nav.Link eventKey="job-queue">
                                            <span className="pl-1 menu-name">Job Queue</span>
                                        </Nav.Link>
                                    </Nav.Item>
                                    <Nav.Item>
                                        <Nav.Link eventKey="input-data-queue" >
                                            <span className="pl-1 menu-name">Input Data Queue</span>
                                        </Nav.Link>
                                    </Nav.Item>
                                    <Nav.Item>
                                        <Nav.Link eventKey="output-data-queue" >
                                            <span className="pl-1 menu-name">Output Data Queue</span>
                                        </Nav.Link>
                                    </Nav.Item>
                                    <Nav.Item>
                                        <Nav.Link eventKey="order-event-queue" >
                                            <span className="pl-1 menu-name">Order Event Queue</span>
                                        </Nav.Link>
                                    </Nav.Item>
                                    <Nav.Item>
                                        <Nav.Link eventKey="inventory-event-queue" >
                                            <span className="pl-1 menu-name">Inventory Event Queue</span>
                                        </Nav.Link>
                                    </Nav.Item>
                                </Nav>
                                <Tab.Content>
                                    <Tab.Pane eventKey="job-queue">
                                        <JobQueuePanel tenant={props.tenant} />
                                    </Tab.Pane>
                                    <Tab.Pane eventKey="input-data-queue">
                                        <InputDataQueuePanel tenant={props.tenant} />
                                    </Tab.Pane>
                                    <Tab.Pane eventKey="output-data-queue">
                                        <OutputDataQueuePanel tenant={props.tenant} />
                                    </Tab.Pane>
                                    <Tab.Pane eventKey="order-event-queue">
                                        <OrderEventQueuePanel tenant={props.tenant} />
                                    </Tab.Pane>
                                    <Tab.Pane eventKey="inventory-event-queue">
                                        <InventoryEventQueuePanel tenant={props.tenant} />
                                    </Tab.Pane>
                                </Tab.Content>
                            </Tab.Container>
                        </Col>
                    </Card.Body>
                </Card>
            </Col>
        </>
    );
}

export default QueuesPanelContainer;