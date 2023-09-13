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

import TenantList from './../../../Common/TenantList/TenantList';

import { useAppState } from "../../../../state";
import QueuesPanelContainer from './QueuesPanelContainer';

import './QueuesPage.scss';


const QueuesPage = (props) => {

    const { user, app, onAppStateRefresh } = useAppState();

    const [selectedTenant, setSelectedTenant] = useState();

    const handleSelectTenant = (tenant) => {
        setSelectedTenant(tenant);
    }

    return (
        <Container fluid className="setupView" id="queuesPage">
            <Row>
                <TenantList user={user} onSelectTenant={(t) => handleSelectTenant(t)} selectedTenant={selectedTenant} />
                {selectedTenant && <QueuesPanelContainer tenant={selectedTenant} />}
            </Row>
        </Container>
    );
}

export default QueuesPage;