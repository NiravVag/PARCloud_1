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

import TenantList from '../../Common/TenantList/TenantList';

import ItemView from './ItemView';

import '../TenantSetup.scss';

import { useAppState } from "../../../state";

const ItemViewPage = (props) => {
    const { user, app, onAppStateRefresh } = useAppState();

    const [selectedTenant, setSelectedTenant] = useState();

    const [isActive, setActive] = useState(false);

    let tenantSetup = undefined;

    const handleSelectTenant = (tenant) => {
        setSelectedTenant(tenant);
    }


    if (selectedTenant) {
        tenantSetup = (<ItemView tenant={selectedTenant} onBackButtonClick={() => handleSelectTenant()} />);
    }

    return (
        <>
            <Container fluid className="setupView" id="ItemViewPage">

                <Row>


                    <TenantList user={user} onSelectTenant={(t) => handleSelectTenant(t)} selectedTenant={selectedTenant} />

                    {tenantSetup}


                </Row>

            </Container>
        </>
    );
};

export default ItemViewPage;
