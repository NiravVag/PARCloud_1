import React, { useEffect, useState } from 'react';

import ReactTable from '../../../../components/Common/ReactTable/ReactTable';

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

import { CSVLink, CSVDownload } from 'react-csv';

import moment from "moment-timezone";

import PingController from './../../../../pages/Common/PingController';
import ScalesList from './ScalesList';

const ControllerTable = (props) => {

    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState();
    const [requestedAction, setRequestedAction] = useState();

    let startDateValue = "", endDateValue = "";
    if (props.filter.startDate && props.filter.endDate) {
        startDateValue = props.filter.startDate.format("MM/DD/YYYY");
        endDateValue = props.filter.endDate.format("MM/DD/YYYY");
    }

    const url = props.tableMetadata.dataUrl;

    let table = undefined;
    let tableData = [];
    let tableHeader = [];    

    useEffect(() => {        

        const fetchData = async (url, ids) => {
            try {
                setIsLoading(true);

                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ healthCheckIds: ids, filter: props.tableMetadata.filter, dateRangeFilter: props.filter.value, startDate: startDateValue, endDate: endDateValue }),
                });

                if (response.status === 204) {
                    setData([]);
                } else {
                    const data = await response.json();
                    setData(data.healthChecks);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        if (props.tableMetadata.ids && props.tableMetadata.ids.length > 0) {
            fetchData(url, props.tableMetadata.ids);
        }
        else {
            fetchData(url, []);
        }
    }, [url, props.tableMetadata.filter]);

    const handleRetrieveScales = (event) => {
        let scalesModalHeader, scalesModalContent, controllerId = event.target.getAttribute('data-value');

        if (event.target.name.indexOf('registeredScaleCount') >= 0) {
            scalesModalHeader = "Registered Scales";
            scalesModalContent = <ScalesList controllerId={controllerId} registeredScales={true} />;
        } else if (event.target.name.indexOf('onlineScaleCount') >= 0) {
            scalesModalHeader = "Online Scales";
            scalesModalContent = <ScalesList controllerId={controllerId} onlineScales={true} />;
        } else if (event.target.name.indexOf('offlineScaleCount') >= 0) {
            scalesModalHeader = "Offline Scales";
            scalesModalContent = <ScalesList controllerId={controllerId} offlineScales={true} />;
        }

        if (scalesModalHeader && scalesModalContent) {
            props.onOpenModal(scalesModalHeader, scalesModalContent);
        }
    };

    

    if (data && data.length > 0) {
        tableHeader = [
            { label: "Id", key: "id" },
            { label: "Tenant Name", key: "tenantName" },
            { label: "IP Addres", key: "remoteIpAddress" },
            { label: "Network Port", key: "remoteNetworkPort" },
            { label: "TCP Test Status", key: "tcpTestStatus" },
            { label: "Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")" , key: "statusDate" },
            { label: "Router Last Communication (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")", key: "routerLastCommunication" },
            { label: "Status", key: "status" },
            { label: "Registered Scales Count", key: "registeredScaleCount.registeredScaleCount" },
            { label: "Online Scales Count", key: "onlineScaleCount.onlineScaleCount" },
            { label: "Offline Scales Count", key: "offlineScaleCount.offlineScaleCount" },
        ];

        tableData = data.map((prop, key) => {
            let modalHeader = "Ping Controller",
                modalContent = <PingController ipAddress={prop.remoteIpAddress} networkPort={prop.remoteNetworkPort} tenantId={prop.tenantId} onCancel={props.onCloseModal}/>;
            
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
                    </Button>
                </>
            );

            if (requestedAction && requestedAction.actionId == prop.routerAdress) {
                actionButtons = <Spinner animation="border" variant="primary" />;
            }

            return {
                id: prop.id,
                controllerId: prop.controllerId,
                tenantName: prop.tenantName,
                remoteIpAddress: prop.remoteIpAddress,
                remoteNetworkPort: prop.remoteNetworkPort,
                tcpTestStatus: prop.tcpTestStatus,
                statusDate: (prop.modified == null) ? "" : moment.utc(prop.modified).local().format("MM/DD/YYYY hh:mm:ss A z"),
                previousStatus: prop.previousStatus,
                previousStatusDate: (prop.previousStatusDate == null) ? "" : moment.utc(prop.previousStatusDate).local().format("MM/DD/YYYY hh:mm:ss A"),
                routerLastCommunication: (prop.routerLastCommunication == null) ? "" : moment.utc(prop.routerLastCommunication).local().format("MM/DD/YYYY hh:mm:ss A"),
                status: prop.status,
                registeredScaleCount: { id: prop.id, controllerId: prop.controllerId, registeredScaleCount: prop.registeredScaleCount },
                onlineScaleCount: { id: prop.id, controllerId: prop.controllerId, onlineScaleCount: prop.onlineScaleCount },
                offlineScaleCount: { id: prop.id, controllerId: prop.controllerId, offlineScaleCount: prop.offlineScaleCount },
                actions: (
                    <div className="actions-right">
                        {actionButtons}
                    </div>
                ),
            };
        });
    }

    table = (
        <ReactTable
            data={tableData}
            columns={[
                {
                    Header: "Id",
                    accessor: "id",
                    className: 'rt-custom',
                    maxWidth: 30,
                    minWidth: 30,
                    width: 60,
                },
                {
                    Header: "Tenant Name",
                    accessor: "tenantName",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,
                    width: 100,
                },
                {
                    Header: "IP Address",
                    accessor: "remoteIpAddress",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150 ,
                    width: 150,
                },
                {
                    Header: "Network Port",
                    accessor: "remoteNetworkPort",
                    className: 'rt-custom',
                    maxWidth: 120,
                    minWidth: 100,
                    width: 60,
                },
                {
                    Header: "TCP Test Status",
                    accessor: "tcpTestStatus",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,
                    width: 70,
                },
                {
                    Header: "Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")" ,
                    accessor: "statusDate",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                //{
                //    Header: "Previous Status",
                //    accessor: "previousStatus",
                //    className: 'rt-custom',
                //    maxWidth: 70,
                //    minWidth: 70,

                //    width: 70,
                //},
                //{
                //    Header: "Previous Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                //    accessor: "previousStatusDate",
                //    className: 'rt-custom',
                //    maxWidth: 200,
                //    minWidth: 150,
                //    width: 150,
                //},
                {
                    Header: "Router Last Communication (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "routerLastCommunication",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Status",
                    accessor: "status",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,
                    width: 70,
                },
                {
                    Header: "Registered Scales Count",
                    accessor: "registeredScaleCount",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                    Cell: ({ cell: { value } }) => (
                        <a
                            name={`registeredScaleCount-` + value.controllerId}
                            data-value={value.controllerId}
                            className="counts-link"
                            onClick={(value.registeredScaleCount > 0) ? handleRetrieveScales : undefined}
                        >{value.registeredScaleCount}</a>
                    )
                },
                {
                    Header: "Online Scales Count",
                    accessor: "onlineScaleCount",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                    Cell: ({ cell: { value } }) => (
                        <a
                            name={`onlineScaleCount-` + value.controllerId}
                            data-value={value.controllerId}
                            className="counts-link"
                            onClick={(value.onlineScaleCount > 0) ? handleRetrieveScales : undefined}
                        >{value.onlineScaleCount}</a>
                    )
                },
                {
                    Header: "Offline Scales Count",
                    accessor: "offlineScaleCount",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                    Cell: ({ cell: { value } }) => (
                        <a
                            name={`offlineScaleCount-` + value.controllerId}
                            data-value={value.controllerId}
                            className="counts-link"
                            onClick={(value.offlineScaleCount > 0) ? handleRetrieveScales : undefined}
                        >{value.offlineScaleCount}</a>
                    )
                },
                {
                    Header: "Actions",
                    accessor: "actions",
                    className: 'rt-custom',
                    sortable: false,
                    filterable: false,
                    maxWidth: 70,
                    minWidth: 70,
                    width: 70,
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
            <Card className="dashboard-table-card">
                <Card.Header className="d-flex justify-content-between">
                    <Card.Title as="h4">{props.title}</Card.Title>
                    <CSVLink
                        data={tableData}
                        headers={tableHeader}
                        filename={"download_file.csv"}
                        className="bg-success btn btn-default btn-sm"
                        target="_blank"
                    >
                        <i className="fas fa-table mr-2"></i>
                        Export to CSV
                     </CSVLink>
                </Card.Header>
                <div className="p-1">
                    <div className="dashboard-controller-table">
                        {table}
                    </div>
                </div>
            </Card>
        </>
    );
}

export default ControllerTable;