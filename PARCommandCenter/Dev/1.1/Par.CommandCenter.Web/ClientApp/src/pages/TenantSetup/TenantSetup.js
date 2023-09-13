////import React, { useEffect, useState } from 'react';

////// react-bootstrap components
////import {
////    Badge,
////    Button,
////    ButtonGroup,
////    Card,
////    Form,
////    InputGroup,
////    Navbar,
////    Nav,
////    Table,
////    Container,
////    Row,
////    Col,
////    OverlayTrigger,
////    Tooltip,
////    ListGroup,
////    Modal,
////    Tab

////} from "react-bootstrap";

////import { useAppState } from "../../state";

////import AddUpdateFacility from './Facilities/AddUpdateFacility';
////import FacilitesPanel from './Facilities/FacilitesPanel';

////import AddUpdateController from './Controllers/AddUpdateController';
////import ControllersPanel from './Controllers/ControllersPanel';


////import ScalesPanel from './Scales/ScalesPanel';

////import ItemsPanel from './Items/ItemsPanel';

////import AddRouter from './Routers/AddRouter';
////import RoutersPanel from './Routers/RoutersPanel';
////import TenantSetupTabs from './TenantSetupTabs';

////const tabItems = [
////    { value: "facilities", name: "Facilities", iconClass: "fa fa-building" },
////    { value: "routers", name: "Routers", iconClass: "nc-icon nc-ic_router_48px" },
////    { value: "controllers", name: "Controllers", iconClass: "fa fa-broadcast-tower" },
////    { value: "scales", name: "Scales", iconClass: "fa fa-server" },
////    { value: "items", name: "Items", iconClass: "fa fa-list" },
////];



////const TenantSetup = (props) => {
////    const { user } = useAppState();

////    const tenantName = (props.tenant) ? props.tenant.name : "";
    
////    const [showModal, setShowModal] = useState(false);

////    const [doRefresh, setDoRefresh] = useState(false);
    
////    const [selectedTabValue, setSelectedTabValue] = useState(tabItems[0].value);
////    let panelContent = undefined;
////    let modalContent = undefined;
////    let modalTitle = undefined;

////    const [selectedfacility, setSelectedfacility] = useState();
////    const [selectedController, setSelectedController] = useState();    
    
////    const handleTabItemClick = (event) => {
////        let clickedTabValue = event.currentTarget.getAttribute("tab-value");
////        setSelectedTabValue(clickedTabValue);
////    }

////    const handleRefresh = () => {
////        setShowModal(!showModal)
////        setDoRefresh(!doRefresh)
////    }

////    const handleEditFacilty = (facility) => {
////        setSelectedfacility(facility);
////        setShowModal(!showModal);        
////    }

////    const handleEditController = (controller) => {
////        setSelectedController(controller);
////        setShowModal(!showModal);
////    }

////    const handleModalClose = () => {
////        setSelectedfacility(undefined);
////        setSelectedController(undefined);        
////        handleRefresh();
////    }

////    let actionButton;

////    switch (selectedTabValue) {
////        case "facilities":
////            panelContent = <FacilitesPanel tenant={props.tenant}                
////                onEditFacility={handleEditFacilty}
////                onRefresh={doRefresh} />;
////            modalTitle = "Add Facility";
////            modalContent = <AddUpdateFacility facility={selectedfacility} tenantId={props.tenant.id} onAddComplete={handleRefresh} onCancel={handleModalClose} />;
////            actionButton = (
////                <Button className="text-nowrap green-gradient mr-1" variant="primary" onClick={() => { setShowModal(!showModal) }} >
////                    <span className="btn-label">
////                        <i className="fas fa-plus"></i>
////                    </span>
////                    Add Facility
////                </Button>
////            );
////            break;
////        case "routers":
////            panelContent = <RoutersPanel tenant={props.tenant} onAddRouterClick={() => { setShowModal(!showModal) }} onRefresh={doRefresh} />;
////            modalTitle = "Add Router"
////            modalContent = <AddRouter user={user} tenantId={props.tenant.id} onAddRouterComplete={handleModalClose} />;
////            actionButton = (
////                <Button className="text-nowrap green-gradient mr-1" variant="primary" onClick={() => { setShowModal(!showModal) }}>
////                    <span className="btn-label">
////                        <i className="fas fa-plus"></i>
////                    </span>
////                    Add Router
////                </Button>);
////            break;
////        case "controllers":
////            panelContent = <ControllersPanel tenant={props.tenant} onAddController={() => { setShowModal(!showModal) }} onEditController={handleEditController} onRefresh={doRefresh} />;
////            modalTitle = "Add Controller";
////            modalContent = <AddUpdateController controller={selectedController} tenantId={props.tenant.id} onAddComplete={handleRefresh} onCancel={handleModalClose} />;
////            actionButton = (
////                <Button className="text-nowrap green-gradient mr-1" variant="primary" onClick={() => { setShowModal(!showModal) }} >
////                    <span className="btn-label">
////                        <i className="fas fa-plus"></i>
////                    </span>
////                    Add Controller
////                </Button>
////            );
////            break;
////        case "scales":
////            panelContent = <ScalesPanel tenant={props.tenant} />;
////            modalTitle = "";       
////            break;
////        case "items":
////            panelContent = <ItemsPanel tenant={props.tenant} />;
////            modalTitle = "";       
////            break;    
////    }

////    return (
////        <Col md="9" id="tenantSetup">
////            <button type="button" className="p-0 backButton btn-fill btn-round  d-none bg-none border-0 btn text-secondary" onClick={props.onBackButtonClick}>
////                <i className="fa-15x fas fa-arrow-left"></i>
////            </button>
////            <div className="bg-white p-2 border-0 card">
////                <Card.Header>
////                    <div className="row  justify-content-between">
////                        <Card.Title className="col-6 pb-2 m-0">
                  
////                            <h4 className="mt-0">
////                                {tenantName}
////                                </h4>
                        
////                        </Card.Title>
////                        <div className="col-6 text-right">{actionButton}</div>
////                    </div>
////                    <div className="row  justify-content-between">
////                        <TenantSetupTabs tabs={tabItems}  selectedValue={selectedTabValue} onTabItemClick={handleTabItemClick} />
////                    </div>
////                </Card.Header>
////                {panelContent}
////            </div>
////            <Modal
////                size="fluid"
////                show={showModal}
////                onHide={handleModalClose}
////            >
////                <Modal.Header closeButton>
////                    <h4 className="modal-title text-center">
////                        {modalTitle}
////                    </h4>
                  
////                </Modal.Header>
////                <Modal.Body>
////                    {modalContent}
////                </Modal.Body>
////            </Modal>
////        </Col>
////    );
////};

////export default TenantSetup;
