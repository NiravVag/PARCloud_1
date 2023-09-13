import React, { useEffect, useState } from 'react';

import ReactTable from '../../../components/Common/ReactTable/ReactTable';

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

const InterfaceTable = (props) => {

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
    

    const handleResetInterfaceMessage = async (interfaceType, id) => {

        let action = { type: interfaceType, actionId: id };


        setRequestedAction(action);

        try {
            const response = await fetch("api/interface/ResetInterfaceEvent/" + interfaceType + "/" + id,
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({})
                });

            

            if (response.status === 204) {
            } else if (response.status === 500) {
            }
            else {
                const data = await response.json();
            }
        } catch (error) {
            throw error;
        }

        setRequestedAction();
    }
        
    const handleDeleteInterfaceMessage = async (interfaceType, id) => {

        let action = { type: interfaceType, actionId: id};


        setRequestedAction(action);

        try {
            const response = await fetch("api/interface/DeleteInterfaceEvent/" + interfaceType + "/" + id,
                {
                    method: "delete",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ })
                });


            if (response.status === 204) {
            } else if (response.status === 500) {
            }
            else {
                const data = await response.json();
            }
        } catch (error) {
            throw error;
        }

        setRequestedAction();
    }

    if (data && data.length > 0) {
        tableHeader = [
            { label: "Id", key: "id" },
            { label: "Tenant Name", key: "tenantName"},
            { label: "Interface Type", key: "interfaceType"},
            { label: "Status", key: "status"},
            { label: "Status Date", key: "statusDate"},
            { label: "Description", key: "description" },
            { label: "Published At", key: "published" },
            { label: "Sent At", key: "sent" },
            { label: "Started At", key: "started" },
            { label: "External System", key: "externalSystemName"},
            { label: "fileName", key: "fileName"},
            { label: "File Location", key: "fileLocation" },
            { label: "Mime Type", key: "mimeType" }
        ];

        tableData = data.map((prop, key) => {
            let actionButtons = (
                <>
                    <a title="Reset Interface Message">
                        <i className="text-primary mr-2 fa fa-redo fa-lg" onClick={(e) => handleResetInterfaceMessage(prop.interfaceType, prop.id)} />
                    </a>
                    <a title="Delete Interface Message">
                        <i className="text-danger fa fa-times fa-lg" onClick={(e) => handleDeleteInterfaceMessage(prop.interfaceType, prop.id)} />
                    </a>
                </>
            );            

            if (requestedAction && requestedAction.actionId == prop.id ) {
                actionButtons = <Spinner animation="border" variant="primary" />;
            }

            return {
                id: prop.id,
                tenantName: prop.tenantName,
                interfaceType: prop.interfaceType,
                status: prop.status,
                statusDate: (prop.modified == null) ? "" : moment.utc(prop.modified).local().format("MM/DD/YYYY hh:mm:ss A"),
                description: prop.errorMessage,
                published: (prop.published == null) ? "" : moment.utc(prop.published).local().format("MM/DD/YYYY hh:mm:ss A"),
                sent: (prop.sent == null) ? "" : moment.utc(prop.sent).local().format("MM/DD/YYYY hh:mm:ss A"),
                published: (prop.published == null) ? "" : moment.utc(prop.published).local().format("MM/DD/YYYY hh:mm:ss A"),
                started: (prop.started == null) ? "" : moment.utc(prop.started).local().format("MM/DD/YYYY hh:mm:ss A"),
                externalSystemName: prop.externalSystemName,
                fileName: prop.fileName,
                fileLocation: props.fileLocation,
                mimeType: props.mimeType,
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
                    maxWidth: 60,
                    minWidth: 60,
                    width: 60,
                },
                {
                    Header: "Tenant Name",
                    accessor: "tenantName",
                    className: 'rt-first',
                    maxWidth: 200,
                    minWidth: 200,
                    width: 200,
                },
                {
                    Header: "Interface Type",
                    accessor: "interfaceType",
                    className: 'rt-first',
                    maxWidth: 200,
                    minWidth: 110 ,
                    width: 110,
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
                    Header: "Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "statusDate",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Description",
                    accessor: "description",
                    className: 'rt-first',
                    maxWidth: 200,
                    minWidth: 200,
                    width: 200,
                },
                {
                    Header: "Sent Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "published",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 200,
                    width: 200,
                },
                //{
                //    Header: "Sent At",
                //    accessor: "sent",
                //    className: 'rt-custom',
                //    maxWidth: 200,
                //    minWidth: 200,
                //    width: 200,
                //},               
                //{
                //    Header: "Started At",
                //    accessor: "started",
                //    className: 'rt-custom',
                //    maxWidth: 200,
                //    minWidth: 200,
                //    width: 200,
                //},
                {
                    Header: "External System",
                    accessor: "externalSystemName",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,

                    width: 150,
                },
                {
                    Header: "fileName",
                    accessor: "fileName",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,

                    width: 150,
                },
                {
                    Header: "File Location",
                    accessor: "fileLocation",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,

                    width: 150,
                },
                {
                    Header: "Mime Type",
                    accessor: "mimeType",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,

                    width: 70,
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
                <Card.Body>
                    <div className="dashboard-controller-table">
                        {table}
                    </div>
                </Card.Body>
            </Card>
        </>
    );
}

export default InterfaceTable;