import React, { useEffect, useState, useMemo } from 'react';

import ReactTable from "./../../../../components/Common/ReactTable/ReactTable";

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




const RouterControllersList = (props) => {
    const [isLoading, setIsLoading] = useState(true);
    const [controllers, setControllers] = useState([]);
    const [selectedControllers, setSelectedControllers] = useState([]);

    let controllersTable = undefined;
    let data = [];

    const url = "api/Controller/GetByRouterId/" + props.routerId;

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


    const handleControllerSelection = (e) => {
        const controllerId = e.currentTarget.getAttribute("id");
        const checkedValue = e.currentTarget.checked;

        let controllersCpy = [...controllers];        
        if (controllersCpy && controllersCpy.length > 0) {
            for (let controller of controllersCpy) {
                if (controller.id == controllerId) {
                    controller.isSelected = checkedValue;                    
                    break;
                }
            }

            setControllers(controllersCpy);
            let selectedControllers = controllersCpy.filter(x => x.isSelected);
            props.onSelectionChanges(selectedControllers);
        }
    }

    const statusSortFactory = () => {
        const STATUS = { true: 0, false: 1 };

        return (a, b) => {
            const aStatus = a.values.status;
            const bStatus = b.values.status;
            const aVal = STATUS[aStatus];
            const bVal = STATUS[bStatus];
            return aVal < bVal ? -1 : 1;
        };
    };

    const statusSort = useMemo(statusSortFactory);

    if (controllers && controllers.length > 0) {
        data = controllers.map((prop, key) => {

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
               
               
                selectCell: (
                    <Form.Check className="pl-0">
                        <Form.Check.Label>
                            <Form.Check.Input                                
                                type="checkbox"
                                id={prop.id}
                                onChange={handleControllerSelection}
                                checked={prop.isSelected}
                            >
                            </Form.Check.Input>
                            <span className="form-check-sign"></span>
                        </Form.Check.Label>
                    </Form.Check>
                ),
            };
        });
    }

    controllersTable = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "",
                    accessor: "selectCell",
                    className: 'rt-custom',
                    sortable: false,
                    filterable: false,
                    maxWidth: 30,
                    minWidth: 30,
                    width: 30,

                },
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
                }
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <div id="router-controllers">
            <Card.Body className="table-full-width p-0">
                <Row>
                    <Col className="mx-auto router-controllers-table pt-2">
                        {controllersTable}
                    </Col>
                </Row>
            </Card.Body>
        </div>
    );
}

export default RouterControllersList;
