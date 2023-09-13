import React, { useState } from 'react';

import { useHistory } from "react-router-dom";

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

import TenantsSummaryPanel from "../Summary/TenantsSummaryPanel";

import './TenantsSummaryPage.scss';

const TenantsSummaryPage = (props) => {
    const { user, app, onAppStateRefresh } = useAppState();
    const history = useHistory();    

    const handleTenantSetupHub = (tenantId, hubName) => {
        history.push("/" + hubName + "?tenantId=" + tenantId);        
    }

    return (
        <>
            <Container fluid id="tenantsSummaryPage">
                <Row>
                    <Col md="12" >
                        <div className="bg-white p-2 border-0 card">
                            <TenantsSummaryPanel onOpenHub={handleTenantSetupHub} />
                        </div>
                    </Col>
                </Row>
            </Container>
        </>
    );
};

export default TenantsSummaryPage;
