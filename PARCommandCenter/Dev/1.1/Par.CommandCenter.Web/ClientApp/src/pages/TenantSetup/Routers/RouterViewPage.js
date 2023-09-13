
import React, { useState } from 'react';
import { useLocation } from 'react-router';
import queryString from 'query-string';
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

import TenantList from '../../Common/TenantList/TenantList';

import RouterView from './RouterView';

import '../TenantSetup.scss';

import { useAppState } from "../../../state";

const RouterViewPage = (props) => {
    const { user, app, onAppStateRefresh } = useAppState();

    const history = useHistory();

    let intTenant;
    const location = useLocation();
    const queryParams = queryString.parse(location.search);
    if (queryParams && queryParams.tenantId && parseInt(queryParams.tenantId) > 0) {
        intTenant = { id: parseInt(queryParams.tenantId) }
    }

    const [selectedTenant, setSelectedTenant] = useState(intTenant);

    const [isActive, setActive] = useState(false);

    let tenantSetup = undefined;

    const handleSelectTenant = (tenant) => {
        setSelectedTenant(tenant);
        history.push(location.pathname); 
    }


    if (selectedTenant) {
        tenantSetup = (<RouterView tenant={selectedTenant} onBackButtonClick={() => handleSelectTenant()} />);
    }

    return (
        <>
            <Container fluid className="setupView" id="RouterViewPage">

                <Row>
                    <TenantList user={user} onSelectTenant={(t) => handleSelectTenant(t)} selectedTenant={selectedTenant} />

                    {tenantSetup}

                </Row>

            </Container>
        </>
    );
};

export default RouterViewPage;
