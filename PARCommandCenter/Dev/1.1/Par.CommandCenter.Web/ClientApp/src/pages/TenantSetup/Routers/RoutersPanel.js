import React, { useEffect, useState, useMemo } from 'react';

import ReactTable from './../../../components/Common/ReactTable/ReactTable';

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

import moment from "moment-timezone";

const RoutersPanel = (props) => {
    const [alert, setAlert] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [routers, setRouters] = useState([]);
    const [removeRouter, setRemoveRouter] = useState();
    const [restartRouter, setRestartRouter] = useState(false);
    const [requestedAction, setRequestedAction] = useState();
    const [restartError, setRestartError] = useState();

    let routersTable = undefined;
    let data = []; 

    const url = "api/Router/GetByTenantId/" + props.tenant.id;

    useEffect(() => {        
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setRouters([]);
                } else {
                    const data = await response.json();
                    setRouters(data.routers);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, removeRouter, restartRouter, props.doRefresh]);

    ////const handleRestartRouterService = async (routerAddress) => {

    ////    let action = { type: 'restartRouter', actionId: routerAddress };


    ////    setRequestedAction(action);

    ////    try {
    ////        const response = await fetch("api/router/restart",
    ////            {
    ////                method: "post",
    ////                headers: new Headers({
    ////                    "Content-Type": "application/json",
    ////                    Accept: "application/json"
    ////                }),
    ////                body: JSON.stringify({ address: routerAddress })
    ////            });


    ////        if (response.status != 200) {
    ////            setRestartError("Restart router not working");
    ////        } else {
    ////            const responseBody = await response.json();

                

    ////            if (!responseBody.success) {
    ////                setRestartError("Restart router not working");
    ////            }
    ////        }

    ////        ////if (response.status === 204) {
    ////        ////} else if (response.status === 500) {
    ////        ////}
    ////        ////else {
    ////        ////    const data = await response.json();
    ////        ////}
    ////    } catch (error) {
    ////        throw error;
    ////    } 

    ////    setTimeout(() => {
    ////        setRequestedAction();
    ////        setRestartRouter(!restartRouter);

    ////    }, 3000);
    ////}


    const handleRemoveRouter = async (router) => {
        hideAlert();
        setRemoveRouter(router);
        try {
            const url = "api/Router/Delete";
           

            const response = await fetch(url,
                {
                    method: "delete",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: router.address})
                }
            );

            if (response.status === 400) {
                let responseBody = await response.json();                
                setRemoveRouter(undefined);
            } else if (response.status === 200) {
                let responseBody = await response.json();
                setRemoveRouter(undefined);
                if (responseBody.success == true) {
                    const deleteServiceUrl = "api/Router/DeleteService";
                    const removeServiceResponse = await fetch(deleteServiceUrl,
                        {
                            method: "delete",
                            headers: new Headers({
                                "Content-Type": "application/json",
                                Accept: "application/json"
                            }),
                            body: JSON.stringify({ address: router.address })
                        }
                    );

                    responseBody = await removeServiceResponse.json();
                }
            } 
        } catch (e) {
            console.log(e);
            setRemoveRouter(undefined);
        }

        setRemoveRouter(undefined);
    }

    const confirmRemoveRouterMessage = (router) => {
        setAlert(
            <SweetAlert                
                style={{ display: "block", marginTop: "-100px" }}
                title='Are you sure?'
                onConfirm={() => handleRemoveRouter(router)}
                onCancel={() => hideAlert()}
                confirmBtnBsStyle=" btn-medium btn-danger"
                cancelBtnBsStyle=" btn-medium btn-secondary"
                confirmBtnText="Yes, delete it!"
                cancelBtnText="Cancel"
                showCancel
            >
                This router will be deleted. You can't undo this action
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

    if (routers && routers.length > 0) {
        data = routers.map((prop, key) => {

            let actionButtons = (
                <>  
                    <Button
                        onClick={(e) => {
                            props.onOpenRouterUtility(prop.id, prop.address, prop.deviceTypeDisplayName);
                        }}
                        variant="primary"
                        className="btn-primary"
                        size="sm"
                    >
                        <i className="fas fa-tools fa-lg" />
                    </Button>{" "}
                    <Button onClick={() => {
                        let obj = routers.find((o) => o.id === prop.id);
                        confirmRemoveRouterMessage(obj);
                    }}
                        title="Delete Router"
                        variant="danger"
                        size="sm"
                        className="btn-link remove text-danger"
                    >
                        <i className="fa fa-times" />
                    </Button>
                </>
            );

            if ((removeRouter && removeRouter.id == prop.id) || (requestedAction && requestedAction.actionId == prop.address)) {
                actionButtons = <Spinner animation="border" variant="primary" />;
            }

            return {
                id: prop.id,
                address: prop.address,
                firmwareVersion: prop.firmwareVersion,
                lastCommunication: moment.utc(prop.lastCommunication).local().format("MM/DD/YYYY hh:mm:ss A"),
                lastReboot: moment.utc(prop.lastReboot).local().format("MM/DD/YYYY hh:mm:ss A"),
                status: prop.isRunning,
                routerType: prop.deviceTypeDisplayName,
                computerName: prop.computerName,
                serviceName: prop.serviceName,
                serviceDisplayName: prop.serviceDisplayName,
                statusContent: (prop.isRunning) ?
                    <Alert variant="success" className="status">
                        <span>Online</span>
                    </Alert>
                    :
                    <Alert variant="danger" className="status">
                        <span>Offline</span>
                    </Alert>,
                actions: (
                    <div className="actions-right">
                        {/*<Button*/}
                        {/*    variant="warning"*/}
                        {/*    size="sm"*/}
                        {/*    className="text-warning btn-link edit"*/}
                        {/*>*/}
                        {/*    <i className="fa fa-edit" />*/}
                        {/*</Button>{" "}*/}
                        {actionButtons}
                    </div>
                ),
            };
        });
    }

    routersTable = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Address",
                    accessor: "address",
                    
                    className: 'rt-first',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Firmware Version",
                    accessor: "firmwareVersion",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },
                {
                    Header: "Router Type",
                    accessor: "routerType",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 80,
                },
                {
                    Header: "Computer Name",
                    accessor: "computerName",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 80,
                },
                {
                    Header: "Service Name",
                    accessor: "serviceName",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 80,
                },
                {
                    Header: "Service Dispaly Name",
                    accessor: "serviceDisplayName",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 80,
                },
                {
                    Header: "Last Communication (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "lastCommunication",
                    className: 'rt-custom',
                    maxWidth: 105,
                    minWidth: 90,
                    width: 160,
                },
                {
                    Header: "Last Reboot (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "lastReboot",
                    filterable: false,
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 160,
                    width: 140,
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
                    minWidth: 115,
                },
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <> 
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto setup-routers-table pt-2">
                        {routersTable}
                    </Col>
                </Row>
            </Card.Body>            
            {alert}
        </>
    );
}

export default RoutersPanel;
