import React, { useEffect, useState, useMemo } from 'react';

import ReactTable from '../../../components/Common/ReactTable/ReactTable';

import SweetAlert from 'react-bootstrap-sweetalert';

import {
    Alert,
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
    FormControl,
    Spinner
} from "react-bootstrap";


import PingController from './../../Common/PingController';

import './ControllerSetup.scss'

const ControllersPanel = (props) => {
    const [alert, setAlert] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [controllers, setControllers] = useState([]);

    let controllersTable = undefined;
    let data = [];

    const url = "api/Controller/GetByTenantId/" + props.tenant.id;

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setControllers([]);
                } else {
                    const data = await response.json();
                    setControllers(data.controllers);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, props.onRefresh]);

    const noExitingRoutersMessage = () => {
        setAlert(
            <SweetAlert
                style={{ display: "block", marginTop: "-100px" }}
                title="There is no exisiting Routers!"
                onConfirm={() => hideAlert()}
                onCancel={() => hideAlert()}
                confirmBtnBsStyle="info"
            >
                You need to create a cloud router in order to add controllers.
            </SweetAlert>
        );
    };

    const hideAlert = () => {
        setAlert(null);
    };

    const statusSortFactory = () => {
        const STATUS = { true: 0, false: 1 };
        const sortFunc = (a, b) => {
            const aStatus = a.values.status;
            const bStatus = b.values.status;
            const aVal = STATUS[aStatus];
            const bVal = STATUS[bStatus];
            return aVal < bVal ? -1 : 1;
        };
        return sortFunc;
    };

    const statusSort = useMemo(statusSortFactory);

    if (controllers && controllers.length > 0) {
        data = controllers.map((prop, key) => {            
            let modalHeader = "Ping Controller",
                modalContent = <PingController ipAddress={prop.ipAddress} networkPort={prop.networkPort} tenantId={props.tenant.id} onCancel={props.onCloseModal} />;

            let actionButtons = (
                <>
                    <Button
                        onClick={(e) => {
                            props.onOpenModal(modalHeader, modalContent);
                        }}
                        variant="primary"
                        className="btn-primary"
                        size="sm"
                    >
                        <i className="fas fa-tools fa-lg" />
                    </Button>{" "}
                    <Button
                        onClick={() => {
                            props.onEditController(prop);
                        }
                        }
                        variant="warning"
                        size="sm"
                        className="text-warning btn-link edit"
                    >
                        <i className="fa fa-edit" />
                    </Button>
                </>
            );

            return {
                id: prop.id,
                routerAddress: prop.router.address,
                portName: prop.portName,
                ipAddress: prop.ipAddress,
                networkPort: prop.networkPort,
                macAddress: prop.macAddress,
                parChargeMode: (prop.parChargeMode) ? "ON" : "OFF",
                parChargeBatch: (prop.parChargeBatch) ? "ON" : "OFF",
                status: prop.isRunning,
                statusContent: (prop.isRunning) ?
                    <Alert variant="success" className="status">
                        <span>Online</span>
                    </Alert>
                    :
                    <Alert variant="danger" className="status">
                        <span>Offline</span>
                    </Alert>,
                scalesLocations: prop.scalesLocations,
                active: (prop.active && prop.active == true) ? "Yes" : "No",
                actions: (
                    <div className="actions-right">
                        {actionButtons}
                        {/*<Button*/}
                        {/*    variant="danger"*/}
                        {/*    size="sm"*/}
                        {/*    className="btn-link remove text-danger"*/}
                        {/*>*/}
                        {/*    <i className="fa fa-times" />*/}
                        {/*</Button>{" "}*/}
                    </div>
                ),
            };
        });
    }

    controllersTable = (
        <ReactTable            
            data={data}
            columns={[
                {
                    Header: "Router Address",
                    accessor: "routerAddress",
                    className: 'rt-first',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                    
                },
                {
                    Header: "IP",
                    accessor: "ipAddress",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },
                {
                    Header: "Mac",
                    accessor: "macAddress",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Port Name",
                    accessor: "portName",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },                
                {
                    Header: "Network Port",
                    accessor: "networkPort",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },                
                {
                    Header: "Par Charge",
                    accessor: "parChargeMode",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },
                {
                    Header: "Par Charge Batch",
                    accessor: "parChargeBatch",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                   
                },                
                {
                    Header: "Scale Locations",
                    accessor: "scalesLocations",
                    className: 'rt-custom',                    
                    maxWidth: 150,
                    minWidth: 100,
                    width: 150,
                },
                {
                    Header: "Active",
                    accessor: "active",
                    className: 'rt-custom',                    
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },
                {
                    Header: "StatusHidden",
                    accessor: "status",
                    className: 'rt-custom',
                    show: false,
                },
                {
                    Header: "Status",
                    accessor: "statusContent",
                    className: 'rt-custom',
                    sortType: statusSort,
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },
                {
                    Header: "Actions",
                    accessor: "actions",
                    className: 'rt-custom',
                    sortable: false,
                    filterable: false,
                    maxWidth: 100,
                    minWidth: 100,

                    width: 100,
                },
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <div id="controllerSetup">
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto setup-controllers-table pt-2">
                        {controllersTable}
                    </Col>
                </Row>
            </Card.Body>            
            {alert}
        </div>
    );
}

export default ControllersPanel;
