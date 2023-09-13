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

import FacilityView from './FacilityView';

import '../TenantSetup.scss';

import { useAppState } from "../../../state";

const FacilityViewPage = (props) => {    
    const { user, app, onAppStateRefresh } = useAppState();
    const history = useHistory();

    let intTenant;
    const location = useLocation();
    const queryParams = queryString.parse(location.search);
    if (queryParams && queryParams.tenantId && parseInt(queryParams.tenantId) > 0) {
        intTenant = { id: parseInt(queryParams.tenantId) }
    }

    const [selectedTenant, setSelectedTenant] = useState(intTenant);

    let tenantSetup = undefined;

    const handleSelectTenant = (tenant) => {        
        setSelectedTenant(tenant);
        history.push(location.pathname); 
    }

    if (selectedTenant) {
        tenantSetup = (<FacilityView tenant={selectedTenant} onBackButtonClick={() => handleSelectTenant()} />);
    }

    return (
        <>
            <Container fluid  className="setupView" id="FacilityViewPage">

                <Row>
                    <TenantList user={user} onSelectTenant={(t) => handleSelectTenant(t)} selectedTenant={selectedTenant} />

                    {tenantSetup}
                </Row>

            </Container>
        </>
    );
};

export default FacilityViewPage;
