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
    ListGroup
} from "react-bootstrap";

import { useAppState } from "../../../state";

import HL7CloudServersPanel from "../HL7CloudServers/HL7CloudServersPanel";


const HL7CloudServersPage = (props) => {
    const { user, app, onAppStateRefresh } = useAppState();
 
    return (
        <>
            <Container fluid className="setupView" id="HL7CloudServersPage">

                <Row>
                    <Col md="12" id="FacilityView">                        
                        <div className="bg-white p-2 border-0 card">
                            <Card.Header>
                                <div className="row  justify-content-between">
                                    <Card.Title className="col-6 pb-2 m-0">
                                        <h4 className="font-weight-bold m-0">HL7 Cloud Servers</h4>
                                    </Card.Title>
                                </div>
                            </Card.Header>
                            <HL7CloudServersPanel />
                        </div>                       
                    </Col>
                </Row>

            </Container>
        </>
    );
};

export default HL7CloudServersPage;
