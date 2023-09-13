import React, { useEffect, useState } from "react";

// react-bootstrap components
import {
    Badge,
    Button,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Container,
    Col,
    Row,

} from "react-bootstrap";

// react plugin used to create DropdownMenu for selecting items
import Select from "react-select";




import FilterList from "../../components/Common/FilterList";


import "./../../styles/map.scss";
import AuditLogTable from "./AuditLogTable";

//const usersFilterOptions = [
//    { value: "allUsers", label: "All Users" },
//];

const typesFilterOptions = [
    { value: "all", label: "All Types" },
    { value: "created", label: "Created" },
    { value: "edited", label: "Edited" },
    { value: "deleted", label: "Deleted" },
];

const entityTypesFilterOptions = [
    { value: "all", label: "All Types" },
    { value: "controller", label: "Controller" },
];


function AuditLogPage() {

    const [isLoading, setIsLoading] = useState(false);

    const [users, setUsers] = useState();
    const [tenants, setTenants] = useState();

    const [usersFilterOptions, setUsersFilterOptions] = useState([]);
    const [tenantsFilterOptions, setTenantsFilterOptions] = useState([]);

    const [selectedDateFilter, setSelectedDateFilter] = useState({ value: "past24Hours", label: "Past 24 hours" });

    const [selectedUserFilter, setSelectedUserFilter] = useState();
    const [selectedTenantFilter, setSelectedTenantFilter] = useState();
    const [selectedTypeFilter, setSelectedTypeFilter] = useState(typesFilterOptions[0]);
    const [selectedEntityTypeFilter, setSelectedEntityTypeFilter] = useState(entityTypesFilterOptions[0]);

    useEffect(() => {        

        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setUsers([]);
                    setTenants([]);
                } else {
                    const data = await response.json();

                    if (data.users && data.users.length > 0) {
                        setUsers(data.users);

                        let usersOptions = data.users.map(r => ({ value: r.id, label: r.userName }));
                        usersOptions = [{ value: "all", label: "All Users" }, ...usersOptions];

                        setUsersFilterOptions(usersOptions);
                        if (!selectedUserFilter) {
                            setSelectedUserFilter(usersOptions[0]);
                        }
                    }

                    if (data.tenants && data.tenants.length > 0) {
                        setTenants(data.tenants);

                        let tenantsOptions = data.tenants.map(t => ({ value: t.id, label: t.name }));
                        tenantsOptions = [{ value: "all", label: "All Tenants" }, ...tenantsOptions];

                        setTenantsFilterOptions(tenantsOptions);
                        if (!selectedTenantFilter) {
                            setSelectedTenantFilter(tenantsOptions[0]);
                        }
                    }
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        if (!users) {
            let ccUsersURL = "api/User/GetAllCommandCenterUsers";
            fetchData(ccUsersURL);
        }

        if (!tenants) {
            let tenantsURL = "api/Tenant/GetAll";
            fetchData(tenantsURL);
        }
    }, []);

    const handleCustomDateFilter = (filter) => {
        if ((filter.value != "customDate") || (filter.value == "customDate" && filter.startDate && filter.endDate)) {
            setSelectedDateFilter(filter);
        }
    }

    const handleUsersFilter = (option) => {
        setSelectedUserFilter(option);
    }

    const handleTenantsFilter = (option) => {
        setSelectedTenantFilter(option);
    }

    const handleTypesFilter = (option) => {
        setSelectedTypeFilter(option)
    }

    const handleEntityTypesFilter = (option) => {
        setSelectedEntityTypeFilter(option);
    }

    return (
        <Container fluid id="auditLog" className="mb-2">
            <Row className="mt-0">                           
                    <Col md="2">
                        <label className="pt-2">Tenant</label>
                        <Select
                            className="react-select primary"
                            classNamePrefix="react-select"
                            name="tenantsfilterOptions"
                            value={selectedTenantFilter}
                            onChange={(value) => handleTenantsFilter(value)}
                            options={tenantsFilterOptions}
                        />
                    </Col>
                    <Col md="2">
                        <label className="pt-2">Type</label>
                        <Select
                            className="react-select primary"
                            classNamePrefix="react-select"
                            name="usersfilterOptions"
                            value={selectedTypeFilter}
                            onChange={(value) => handleTypesFilter(value)}
                            options={typesFilterOptions}
                        />
                    </Col>
                    <Col md="2">
                        <label className="pt-2">Entity Type</label>
                        <Select
                            className="react-select primary"
                            classNamePrefix="react-select"
                            name="usersfilterOptions"
                            value={selectedEntityTypeFilter}
                            onChange={(value) => handleEntityTypesFilter(value)}
                            options={entityTypesFilterOptions}
                        />
                    </Col>
                    <Col md="2">
                        <label className="pt-2">User</label>
                        <Select
                            className="react-select primary"
                            classNamePrefix="react-select"
                            name="usersfilterOptions"
                            value={selectedUserFilter}
                            onChange={(value) => handleUsersFilter(value)}
                            options={usersFilterOptions}
                        />
                    </Col>
                <Col md="4">
                    <Row>
                        <Col>
                            <label className="pt-2">Date</label>
                            <div className="d-flex" >
                            <FilterList onSelection={handleCustomDateFilter} />
                            </div>
                        </Col>
                    </Row>
                </Col>
            </Row>
            <Row className="mt-3">
                <Col>{selectedUserFilter &&
                    <AuditLogTable tenantFilter={selectedTenantFilter} userFilter={selectedUserFilter} typeFilter={selectedTypeFilter} entityTypeFilter={selectedEntityTypeFilter} dateFilter={selectedDateFilter} />
                    }
                </Col>
            </Row>
            
            
        </Container>
    );
}







export default AuditLogPage;
