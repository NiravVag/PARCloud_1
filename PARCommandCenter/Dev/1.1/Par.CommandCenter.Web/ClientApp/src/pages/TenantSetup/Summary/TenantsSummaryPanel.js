import React, { useEffect, useState } from 'react';

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

import ReactTable from './../../../components/Common/ReactTable/ReactTable';


const TenantsSummaryPanel = (props) => { 
    const [isLoading, setIsLoading] = useState(true);
    const [tenantSummary, setTenantSummary] = useState([]);

    let tableContent = undefined;;
    let data = [];

    const url = "api/Tenant/TenantsSummary";

    useEffect(() => {
        const fetchData = async (url) => {
            try {                
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setTenantSummary([]);
                } else {
                    const data = await response.json();
                    setTenantSummary(data.tenantsSummary);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, props.onRefresh]);

    const handleOpenHubClick = (event) => {
        
        if (!tenantSummary && tenantSummary.length < 1) {
            return;
        }
        let tenantId = event.target.getAttribute('data-value');
        /*let tenant = tenantSummary.find(x => x.id === tenantId)*/
        let linkName = event.target.name;
        let hubName;

        if (tenantId && tenantId > 0) {
            switch (linkName) {
                case "totalFacilities":
                case "totalLocations":
                    hubName = "facilities";
                    break;
                case "totalRouters":
                case "offlineRouters":
                    hubName = "router";
                    break;
                case "totalControllers":
                case "replenishControllers":
                case "chargeControllers":
                case "offlineControllers":
                    hubName = "controller";
                    break;
                case "totalScales":
                case "offlineScales":
                    hubName = "scale";
                    break;
                default:
                    alert('The hub navigation is not supported');
            }
        }
        
        if (hubName && hubName.length > 0) {
            props.onOpenHub(tenantId, hubName);
        }
        
            
       
        ////if (data && data.length > 0) {
        ////    let item = data.find(x => x.id === 'Harvester')
        ////}
    }
   

    if (tenantSummary && tenantSummary.length > 0) {
        data = tenantSummary.map((prop, key) => {
            return {
                id: prop.id,
                name: prop.name,
                totalFacilities: { id: prop.id, name: prop.name, totalFacilities: prop.totalFacilities},
                totalLocations: { id: prop.id, name: prop.name, totalLocations: prop.totalLocations }, 
                totalRouters: { id: prop.id, name: prop.name, totalRouters: prop.totalRouters },
                totalControllers: { id: prop.id, name: prop.name, totalControllers: prop.totalControllers },
                replenishControllers: { id: prop.id, name: prop.name, replenishControllers: prop.replenishControllers },
                chargeControllers: { id: prop.id, name: prop.name, chargeControllers: prop.chargeControllers },
                totalScales: { id: prop.id, name: prop.name, totalScales: prop.totalScales },
                offlineRouters: { id: prop.id, name: prop.name, offlineRouters: prop.offlineRouters },
                offlineControllers: { id: prop.id, name: prop.name, offlineControllers: prop.offlineControllers },
                offlineScales: { id: prop.id, name: prop.name, offlineScales: prop.offlineScales },
                azureVmsSummaryDisplay: prop.azureVmsSummaryDisplay,
            };
        });
    }

    if (isLoading) {
        tableContent =
            <Container className="table-spinner">

                <Spinner animation="border" variant="primary" />

            </Container>
    } else {
        tableContent = (
            <ReactTable
                data={data}
                columns={[
                    {
                        Header: "Tenant Id",
                        accessor: "id",
                        className: 'rt-custom',
                        maxWidth: 100,
                        minWidth: 50,
                        width: 70,
                    },
                    {
                        Header: "Tenant Name",
                        accessor: "name",                        
                        className: 'rt-first',
                        maxWidth: 320,
                        minWidth: 50,
                        width: 250
                    },
                    {
                        Header: "Total Facilities",
                        accessor: "totalFacilities",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,                        
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`totalFacilities`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.totalFacilities}</a>
                        )
                    },
                    {
                        Header: "Total Locations",
                        accessor: "totalLocations",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`totalLocations`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.totalLocations}</a>
                        )
                    },                   
                    {
                        Header: "Total Routers",
                        accessor: "totalRouters",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`totalRouters`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.totalRouters}</a>
                        )
                    },        
                    {
                        Header: "Total Controllers",
                        accessor: "totalControllers",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`totalControllers`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.totalControllers}</a>
                        )
                    },        
                    {
                        Header: "Replenish Controllers",
                        accessor: "replenishControllers",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`replenishControllers`}
                                data-value={value.id}
                                className="counts-link"
                                onClick={handleOpenHubClick}
                            >{value.replenishControllers}</a>
                        )
                    },        
                    {
                        Header: "Charge Controllers",
                        accessor: "chargeControllers",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`chargeControllers`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.chargeControllers}</a>
                        )
                    },
                    {
                        Header: "Total Scales",
                        accessor: "totalScales",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`totalScales` }
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.totalScales}</a>
                        )
                    },
                    {
                        Header: "Offline Routers",
                        accessor: "offlineRouters",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`offlineRouters`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.offlineRouters}</a>
                        )
                    },
                    {
                        Header: "Offline Controllers",
                        accessor: "offlineControllers",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`offlineControllers`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.offlineControllers}</a>
                        )
                    },
                    {
                        Header: "Offline Scales",
                        accessor: "offlineScales",
                        className: 'rt-custom',
                        maxWidth: 50,
                        minWidth: 10,
                        with: 30,
                        Cell: ({ cell: { value } }) => (
                            <a
                                name={`offlineScales`}
                                data-value={value.id}
                                className="counts-link"                                
                                onClick={handleOpenHubClick}
                            >{value.offlineScales}</a>
                        )
                    },
                    {
                        Header: "Azure VMs (# of Routers)",
                        accessor: "azureVmsSummaryDisplay",
                        className: 'rt-custom',
                        maxWidth: 320,
                        minWidth: 50,
                        width: 250
                    }, 
                ]}
                /*
                    You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
                */
                className="-striped -highlight primary-pagination"
            />
        );

    }

    return (
        <>
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto tenants-summary-table pt-2">
                        {tableContent}
                    </Col>
                </Row>
            </Card.Body>
        </>
    );
};

export default TenantsSummaryPanel;
