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




import ScalesPanel from './ScalesPanel';



import TenantSetupTabs from '../TenantSetupTabs';





const ScaleView = (props) => {
    const { user } = useAppState();

    const tenantName = (props.tenant) ? props.tenant.name : "";
    
    return (
        <Col md="9" id="ScaleView">
            <button type="button" className="p-0 backButton btn-fill btn-round  d-none bg-none border-0 btn text-secondary" onClick={props.onBackButtonClick}>
                <i className="fa-15x fas fa-arrow-left"></i>
            </button>
            <div className="bg-white p-2 border-0 card">
                <Card.Header>
                    <div className="row  justify-content-between">
                        <Card.Title className="col-6 pb-2 m-0">
                            <h4 className="font-weight-bold m-0">Scales</h4>
                            <div className="pl-3 row">
                                <i className="pt-1 pr-3 text-primary fas fa-map-marker-alt "></i>
                                <h4 className="mt-0">
                                    {tenantName}
                                </h4>
                            </div>
                        </Card.Title>
                    </div>
                </Card.Header>

                <ScalesPanel tenant={props.tenant} onOpenModal={props.onOpenModal} onCloseModal={props.onCloseModal}  />

            </div>
        </Col>
    );
};

export default ScaleView;
