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
    ListGroup,
    Modal
} from "react-bootstrap";

import TenantList from '../../Common/TenantList/TenantList';

import ControllerView from './ControllerView';

import '../TenantSetup.scss';

import { useAppState } from "../../../state";

const ControllerViewPage = (props) => {
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

    const [showModal, setShowModal] = useState(false);
    const [modalTitle, setModalTitle] = useState("");
    const [modalContent, setModalContent] = useState();

    let tenantSetup = undefined;

    const handleModalClose = () => {
        setShowModal(false);
    }

    const handleModalOpen = (modalHeader, modalContent) => {
        setModalTitle(modalHeader);
        setModalContent(modalContent);
        setShowModal(!showModal);
    }

    const handleSelectTenant = (tenant) => {
        setSelectedTenant(tenant);
        history.push(location.pathname); 
    }


    if (selectedTenant) {
        tenantSetup = (<ControllerView tenant={selectedTenant} onBackButtonClick={() => handleSelectTenant()}
            onOpenModal={handleModalOpen} onCloseModal={handleModalClose}        />);
    }

    return (
        <>
            <Container fluid className="setupView" id="ControllerViewPage">

                <Row>


                    <TenantList user={user} onSelectTenant={(t) => handleSelectTenant(t)} selectedTenant={selectedTenant} />

                    {tenantSetup}


                </Row>
                <Modal
                    size="lg"
                    show={showModal}
                    onHide={handleModalClose}
                >
                    <Modal.Header closeButton>
                        <h4 className="modal-title text-center">
                            {modalTitle}
                        </h4>

                    </Modal.Header>
                    <Modal.Body>
                        {modalContent}
                    </Modal.Body>
                </Modal>

            </Container>
        </>
    );
};

export default ControllerViewPage;
