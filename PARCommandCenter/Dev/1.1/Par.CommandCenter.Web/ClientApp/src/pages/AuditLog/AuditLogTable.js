import React, { useEffect, useState } from 'react';

import ReactTable from '../../components/Common/ReactTable/ReactTable';

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

import './AuditLogPage.scss';


const AuditLogTable = (props) => {

    const [isLoading, setIsLoading] = useState(false);
    const [auditLogsData, setAuditLogsData] = useState();

    let table = undefined;
    let tableData = [];
    let tableHeader = [];

    useEffect(() => {

        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setAuditLogsData([]);
                } else {
                    const data = await response.json();                   

                    if (data.auditLogs) {
                        setAuditLogsData(data.auditLogs);
                    }
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        let allTenantsQuery = "AllTenants=true";
        let tenantIdQuery = "";
        if (props.tenantFilter && props.tenantFilter.value != "all") {
            allTenantsQuery = "AllTenants=false";
            tenantIdQuery = "&tenantId=" + props.tenantFilter.value;
        }

        let allUsersQuery = "AllUsers=true";
        let userIdQuery = "";
        if (props.userFilter && props.userFilter.value != "all") {
            allUsersQuery = "AllUsers=false";
            userIdQuery = "&userId=" + props.userFilter.value;
        }

        let auditLogTypesQuery = "AuditLogTypes=All";
        if (props.typeFilter) {
            auditLogTypesQuery = "AuditLogTypes=" + props.typeFilter.value;
        }

        let auditLogEntityTypesQuery = "AuditLogEntityTypes=All";
        if (props.entityTypeFilter) {
            auditLogEntityTypesQuery = "AuditLogEntityTypes=" + props.entityTypeFilter.value;
        }

        let dateFilterQuery = "";
        let startDateQuery = "", endDateQuery = "";
        if (props.dateFilter) {
            dateFilterQuery = "DateRangeFilter=" + props.dateFilter.value;

            if (props.dateFilter.startDate && props.dateFilter.endDate) {
                startDateQuery = "&startDate=" + props.dateFilter.startDate.format("MM/DD/YYYY");
                endDateQuery = "&endDate=" + props.dateFilter.endDate.format("MM/DD/YYYY");
            }
        }


        let auditLogURL = `api/AuditLog/Get?${allTenantsQuery}${tenantIdQuery}&${allUsersQuery}${userIdQuery}&${auditLogTypesQuery}&${auditLogEntityTypesQuery}&${dateFilterQuery}${startDateQuery}${endDateQuery}`;

        fetchData(auditLogURL);

    }, [props.tenantFilter, props.userFilter, props.typeFilter, props.entityTypeFilter, props.dateFilter]);

    if (auditLogsData) {
        tableHeader = [
            /*{ label: "Id", key: "id" },*/
            { label: "Audit Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")", key: "date" },
            { label: "Audit Type", key: "type" },
            { label: "Entity Type", key: "entityType" },
            { label: "Description", key: "description" },
            { label: "User", key: "userName" },
        ];

        tableData = auditLogsData.map((prop, key) => {
            return {
               /* id: prop.id,*/
                date: (prop.date == null) ? "" : moment.utc(prop.date).local().format("MM/DD/YYYY hh:mm:ss A"),
                type: prop.type,
                entityType: prop.entityType,
                description: prop.description,
                userName: prop.userName,
                tenantName: prop.tenant.name,
            };
        });
    }

    table = (
        <ReactTable
            data={tableData}
            columns={[
                {
                    Header: "Audit Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "date",
                    className: 'rt-first',
                    maxWidth: 150,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Tenant Name",
                    accessor: "tenantName",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 100,
                    width: 150,
                },
                {
                    Header: "Audit Type",
                    accessor: "type",
                    className: 'rt-first',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },
                {
                    Header: "Entity Type",
                    accessor: "entityType",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },
                {
                    Header: "Description",
                    accessor: "description",
                    className: 'rt-first-left',
                    maxWidth: 400,
                    minWidth: 200,
                    width: 400,
                },
                {
                    Header: "User",
                    accessor: "userName",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 200,
                    width: 200,
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
                    <Card.Title as="h4">{``}</Card.Title>
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
                    <div className="audit-log-table">
                        {table}
                    </div>
                </div>

            </Card>
        </>
    );
}

export default AuditLogTable;