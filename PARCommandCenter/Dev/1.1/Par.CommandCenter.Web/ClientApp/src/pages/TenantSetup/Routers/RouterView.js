

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




import AddRouter from './AddRouter';
import AddVirtualCloudRouter from './AddVirtualCloudRouter';
import AddPcCloudRouter from './AddPcCloudRouter';
import ConfigureCloudRouterDevice from './ConfigureCloudRouterDevice';
import RoutersPanel from './RoutersPanel';
import RouterUtilityPanel from "./Utility/RouterUtilityPanel";


const RouterView = (props) => {
    const { user } = useAppState();

    const tenantName = (props.tenant) ? props.tenant.name : "";

    const [showModal, setShowModal] = useState(false);

    const [doRefresh, setDoRefresh] = useState(false);

    const [showAddVMRouter, setShowAddVMRouter] = useState(false);

    const [showAddPcRouter, setShowAddPcRouter] = useState(false);

    const [showConfigureCloudRouterDevice, setShowConfigureCloudRouterDevice] = useState(false);

    const [showRouterUtility, setShowRouterUtility] = useState(false);

    const [utilityRouter, setUtilityRouter] = useState();
    
    
    let panelContent, modalContent, modalTitle, actionButton;    

    const handleRefresh = () => {       
        setDoRefresh(!doRefresh)
    }   

    const handleModalClose = () => {
        setShowAddVMRouter(false);
        setShowAddPcRouter(false);

        setShowConfigureCloudRouterDevice(false);
        setShowModal(false);
        handleRefresh();

        setShowRouterUtility(false);
        setUtilityRouter();
    }

    const handleAddRouterClick = () => {
        setShowAddVMRouter(false);
        setShowAddPcRouter(false);
        setShowConfigureCloudRouterDevice(false);
        setShowModal(true);
    }

    const handleOpenRouterUtilityClick = (routerId, routerAddress, routerType) => {
        setShowAddVMRouter(false);
        setShowAddPcRouter(false);
        setShowConfigureCloudRouterDevice(false);
        setShowRouterUtility(true);
        setShowModal(true);
        setUtilityRouter({ id: routerId, routerAddress: routerAddress, routerType: routerType });
    }

    if (showAddVMRouter) {
        modalTitle = "Create VM Router Setup ";
        modalContent = <AddVirtualCloudRouter onAddRouterComplete={handleModalClose} tenantId={props.tenant.id} />
    }
    else if (showAddPcRouter) {
        modalTitle = "Add CloudRouter On-PC Setup";
        modalContent = <AddPcCloudRouter onAddRouterComplete={handleModalClose} tenantId={props.tenant.id} />
    }
    else if (showConfigureCloudRouterDevice) {
        modalTitle = "Add Router";
        modalContent = <ConfigureCloudRouterDevice onAddRouterComplete={handleModalClose} tenantId={props.tenant.id} />
    } else if (showRouterUtility) {
        modalTitle = "Router Utility -  " + utilityRouter.routerAddress;
        modalContent = <RouterUtilityPanel routerId={utilityRouter.id} routerAddress={utilityRouter.routerAddress} routerType={utilityRouter.routerType} onClose={handleModalClose} />;
    }
    else {
        modalContent = <AddRouter user={user} tenantId={props.tenant.id} onAddRouterComplete={handleModalClose}
            onShowAddVMRouter={() => setShowAddVMRouter(!showAddVMRouter)}
            onShowAddPcRouter={() => setShowAddPcRouter(!showAddPcRouter)}
            onShowConfigureCloudRouterDevice={() => setShowConfigureCloudRouterDevice(!showConfigureCloudRouterDevice)} />;
    }


    panelContent = <RoutersPanel tenant={props.tenant} onAddRouterClick={handleAddRouterClick} onOpenRouterUtility={handleOpenRouterUtilityClick} doRefresh={doRefresh} />;
    
    
    actionButton = (
        <Button className="text-nowrap green-gradient mr-1" variant="primary" onClick={() => { setShowModal(!showModal) }}>
            <span className="btn-label">
                <i className="fas fa-plus"></i>
            </span>
            Add Router
        </Button>
    );
  

    return (
        <Col md="9" id="RouterView">
            <button type="button" className="p-0 backButton btn-fill btn-round  d-none bg-none border-0 btn text-secondary" onClick={props.onBackButtonClick}>
                <i className="fa-15x fas fa-arrow-left"></i>
            </button>

            <div className="bg-white p-2 border-0 card">
                <Card.Header>
                    <div className="row  justify-content-between">
                        <Card.Title className="col-6 pb-2 m-0">
                            <h4 className="font-weight-bold m-0">Routers</h4>
                            <div className="pl-3 row"><i className="pt-1 pr-3 text-primary fas fa-map-marker-alt "></i><h4 className="mt-0">
                                {tenantName}
                            </h4></div>
                        </Card.Title>
                        <div className="col-6 text-right">{actionButton}</div>
                    </div>

                </Card.Header>
                {panelContent}
            </div>

            <Modal
                backdrop="static"
                dialogClassName= "modal-dialog-routers"
                size="fluid"
                show={showModal}
                onHide={handleModalClose}>
                
                <Modal.Header closeButton>
                    <h4 className="modal-title text-center">
                        {modalTitle}
                    </h4>
                </Modal.Header>

                <Modal.Body>
                    {modalContent}
                </Modal.Body>
                   
            </Modal>

        </Col>
    );
};

export default RouterView;
