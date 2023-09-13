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


import AddUpdateController from './AddUpdateController';
import ControllersPanel from './ControllersPanel';

const ControllerView = (props) => {

    const tenantName = (props.tenant) ? props.tenant.name : "";

    const [doRefresh, setDoRefresh] = useState(false);

    const handleControllerUpdateComplate = () => {        
        props.onCloseModal();
        setDoRefresh(!doRefresh);
    }

    const handleEditController = (controller) => {

        let modalHeader = "Edit Controller";
        let modalContent = <AddUpdateController controller={controller} tenantId={props.tenant.id} onAddComplete={handleControllerUpdateComplate} onCancel={props.onCloseModal} />;

        props.onOpenModal(modalHeader, modalContent);
    }

    let panelContent = <ControllersPanel tenant={props.tenant} onRefresh={doRefresh} onOpenModal={props.onOpenModal} onCloseModal={props.onCloseModal} onEditController={handleEditController} />;

    

    return (        
            <Col md="9" id="ControllerView">
                <button type="button" className="p-0 backButton btn-fill btn-round  d-none bg-none border-0 btn text-secondary" onClick={props.onBackButtonClick}>
                    <i className="fa-15x fas fa-arrow-left"></i>
                </button>
                <div className="bg-white p-2 border-0 card">
                    <Card.Header>
                        <div className="row  justify-content-between">
                            <Card.Title className="col-6 pb-2 m-0">
                                <h4 className="font-weight-bold m-0">Controllers</h4>
                                <div className="pl-3 row">
                                    <i className="pt-1 pr-3 text-primary fas fa-map-marker-alt "></i>
                                    <h4 className="mt-0">
                                        {tenantName}
                                    </h4>
                                </div>
                            </Card.Title>
                        <div className="col-6 text-right">
                            <Button className="text-nowrap green-gradient mr-1" variant="primary"
                                onClick={(e) => {
                                    let modalHeader = "Add Controller";
                                    let modalContent = <AddUpdateController tenantId={props.tenant.id} onAddComplete={handleControllerUpdateComplate} onCancel={props.onCloseModal} />;
                                    props.onOpenModal(modalHeader, modalContent);
                                }}
                            >
                                <span className="btn-label">
                                    <i className="fas fa-plus"></i>
                                </span>
                                Add Controller
                            </Button>
                        </div>
                        </div>

                    </Card.Header>
                    {panelContent}
                </div>
                {/*<Modal*/}
                    
                {/*    show={showModal}*/}
                {/*onHide={handleModalClose}*/}
                {/*size="lg"*/}
                {/*>*/}
                {/*    <Modal.Header closeButton>*/}
                {/*        <Modal.Title as="h4">*/}
                {/*            {modalTitle}*/}
                {/*        </Modal.Title>*/}
                {/*    </Modal.Header>*/}
                {/*    <Modal.Body>*/}
                {/*        {modalContent}*/}
                {/*    </Modal.Body>*/}
                {/*</Modal>*/}
            </Col>
    );
};

export default ControllerView;
