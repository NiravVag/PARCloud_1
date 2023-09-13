import React, { useEffect, useState } from 'react';

import ReactTable from '../../../../components/Common/ReactTable/ReactTable';

import TableFilter from "../../../../components/Common/ReactTable/TableFilter";

import moment from "moment";

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
    FormControl,
    Spinner
} from "react-bootstrap";


const ScalesList = (props) => {

    const [isLoading, setIsLoading] = useState(false);

    const [controller, setController] = useState();
    const [scales, setScales] = useState([]);
    

    const [registeredScales, setRegisteredScales] = useState(props.registeredScales ?? false);
    const [onlineScales, setOnlineScales] = useState(props.onlineScales ?? false);
    const [offlineScales, setOfflineScales] = useState(props.offlineScales ?? false);

    let loaderContent;
    let data = [];

    

    useEffect(() => {
        const url = "api/Scale/GetByControllerId/" + props.controllerId + "/" + registeredScales + "/" + onlineScales + "/" + offlineScales;

        const getControllerURL = "api/Controller/GetById/" + props.controllerId + "/true";

        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setScales([]);
                    setController();

                } else {
                    const data = await response.json();
                    if (data.scales && data.scales.length > 0) {
                        setScales(data.scales);
                    } else if (data.controller && data.controller.id > 0) {
                        setController(data.controller);
                    }
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                console.log(error);
                throw error;
            }
        };

        fetchData(url);
        fetchData(getControllerURL);

    }, []);

    if (isLoading) {
        loaderContent = <div
            style={{
                display: "block",
                position: "absolute",
                left: 0,
                right: 0,
                background: "rgba(255,255,255,0.8)",
                transition: "all .3s ease",
                top: 0,
                bottom: 0,
                textAlign: "center"
            }}
        >
            <Spinner animation="border" variant="primary" />
        </div>
    }

    if (scales && scales.length > 0) {
        data = scales.map((prop, key) => {

            return {
                id: prop.id,
                location: prop.locationName,
                address: prop.address,
                itemName: prop.itemName,
                itemNumber: prop.itemNumber,
                scaleWeight: prop.scaleWeight,
                controllerIp: prop.controllerIp,
                lastCommunication: moment.utc(prop.lastCommunication).local().format("MM/DD/YYYY hh:mm:ss A"),
                status: (prop.isRunning) ? "Online" : "Offline",
                
            };
        });
    }   

    let scalesTable = (
        <>
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Location",
                    accessor: "location",
                    className: 'rt-first',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Address",
                    accessor: "address",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Item Name",
                    accessor: "itemName",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                    canFilter: true
                },
                {
                    Header: "Item Number",
                    accessor: "itemNumber",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Scale Weight",
                    accessor: "scaleWeight",
                    className: 'rt-custom',

                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },
                {
                    Header: "Controller IP",
                    accessor: "controllerIp",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Last Communication (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "lastCommunication",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 160,
                },
                {
                    Header: "Status",
                    accessor: "status",
                    className: 'rt-custom',
                    filterable: false,
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            header={TableFilter}
            className="-striped -highlight primary-pagination"
            />
            {loaderContent}
        </>
    );

    return (
        <Container fluid>
            <Row className="mb-3">
                <Col md="12">
                    <div className="w-100 row justify-content-between align-items-center">
                        <h5 className="modal-title text-center">Controller Ip Address: {(controller) ? controller.ipAddress : ""}</h5>
                        <h5 className="modal-title text-center">Status: {(controller && controller.isRunning) ? "Online" : "Offline"}</h5>
                    </div>
                </Col>
            </Row>
            <Row>
                <Col md="12" className="controller-scales-table">
                    {scalesTable}
                    </Col>
                </Row>
        </Container>
    );
}

export default ScalesList;