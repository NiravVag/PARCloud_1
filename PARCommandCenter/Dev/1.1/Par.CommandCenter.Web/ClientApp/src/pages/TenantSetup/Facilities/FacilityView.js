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

import AddUpdateFacility from './AddUpdateFacility';
import FacilitesPanel from './FacilitesPanel';

const FacilityView = (props) => {    

    const tenantName = (props.tenant) ? props.tenant.name : "";

    const [showModal, setShowModal] = useState(false);

    const [doRefresh, setDoRefresh] = useState(false);   

    let panelContent = undefined;
    let modalContent = undefined;
    let modalTitle = undefined;

    const [selectedfacility, setSelectedfacility] = useState();    

    const handleRefresh = () => {
        setShowModal(!showModal)
        setDoRefresh(!doRefresh)
    }

    const handleEditFacilty = (facility) => {
        setSelectedfacility(facility);
        setShowModal(!showModal);
    }  

    const handleModalClose = () => {
        setSelectedfacility(undefined);        
        handleRefresh();
    }

    let actionButton;

    panelContent = <FacilitesPanel tenant={props.tenant}
        onEditFacility={handleEditFacilty}
        onRefresh={doRefresh} />;

    modalTitle = "Add Facility";
    modalContent = <AddUpdateFacility facility={selectedfacility} tenantId={props.tenant.id} onAddComplete={handleRefresh} onCancel={handleModalClose} />;

    actionButton = (
        <Button className="text-nowrap green-gradient mr-1" variant="primary" onClick={() => { setShowModal(!showModal) }} >
            <span className="btn-label">
                <i className="fas fa-plus"></i>
            </span>
            Add Facility
        </Button>
    );

    return (
        <Col md="9" id="FacilityView">
            <button type="button" className="p-0 backButton btn-fill btn-round  d-none bg-none border-0 btn text-secondary" onClick={props.onBackButtonClick}>
                <i className="fa-15x fas fa-arrow-left"></i>
            </button>
            <div className="bg-white p-2 border-0 card">
                <Card.Header>
                    <div className="row  justify-content-between">
                        <Card.Title className="col-6 pb-2 m-0">
                            <h4 className="font-weight-bold m-0">Facilities</h4>
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

export default FacilityView;
