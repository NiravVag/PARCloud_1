import React, { useEffect, useState } from 'react';

// react-bootstrap components
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
    Tab

} from "react-bootstrap";

import { useAppState } from "../../../state";


import ItemsPanel from './ItemsPanel';


import TenantSetupTabs from '../TenantSetupTabs';

const tabItems = [
    { value: "facilities", name: "Facilities", iconClass: "fa fa-building" },
    { value: "routers", name: "Routers", iconClass: "nc-icon nc-ic_router_48px" },
    { value: "controllers", name: "Controllers", iconClass: "fa fa-broadcast-tower" },
    { value: "scales", name: "Scales", iconClass: "fa fa-server" },
    { value: "items", name: "Items", iconClass: "fa fa-list" },
];



const ItemView = (props) => {
    const { user } = useAppState();

    const tenantName = (props.tenant) ? props.tenant.name : "";

    const [showModal, setShowModal] = useState(false);

    const [doRefresh, setDoRefresh] = useState(false);

    const [selectedTabValue, setSelectedTabValue] = useState(tabItems[4].value);
    let panelContent = undefined;
    let modalContent = undefined;
    let modalTitle = undefined;

    const [selectedfacility, setSelectedfacility] = useState();
    const [selectedController, setSelectedController] = useState();

    const handleTabItemClick = (event) => {
        let clickedTabValue = event.currentTarget.getAttribute("tab-value");
        setSelectedTabValue(clickedTabValue);
    }

    const handleRefresh = () => {
        setShowModal(!showModal)
        setDoRefresh(!doRefresh)
    }

    const handleEditFacilty = (facility) => {
        setSelectedfacility(facility);
        setShowModal(!showModal);
    }

    const handleEditController = (controller) => {
        setSelectedController(controller);
        setShowModal(!showModal);
    }

    const handleModalClose = () => {
        setSelectedfacility(undefined);
        setSelectedController(undefined);
        handleRefresh();
    }

    let actionButton;

    switch (selectedTabValue) {
    
        case "items":
            panelContent = <ItemsPanel tenant={props.tenant} />;
            modalTitle = "";
            break;
    }

    return (
        <Col md="9" id="ItemView">
            <button type="button" className="p-0 backButton btn-fill btn-round  d-none bg-none border-0 btn text-secondary" onClick={props.onBackButtonClick}>
                <i className="fa-15x fas fa-arrow-left"></i>
            </button>
            <div className="bg-white p-2 border-0 card">
                <Card.Header>
                    <div className="row  justify-content-between">
                        <Card.Title className="col-6 pb-2 m-0">
                            <h4 className="font-weight-bold m-0">Items</h4>
                            <div className="pl-3 row">
                                <i className="pt-1 pr-3 text-primary fas fa-map-marker-alt "></i>
                                <h4 className="mt-0">
                       
                                {tenantName}
                                </h4>
                                </div>
                        </Card.Title>
                        <div className="col-6 text-right">{actionButton}</div>
                    </div>
                    
                </Card.Header>
                {panelContent}
            </div>
            <Modal
                size="fluid"
                show={showModal}
                onHide={handleModalClose}
            >
                <Modal.Header closeButton>
                    <Modal.Title as="h4">
                        {modalTitle}
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {modalContent}
                </Modal.Body>
            </Modal>
        </Col>
    );
};

export default ItemView;
