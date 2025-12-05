<%@ Page Title="Transfer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="SO_Appraisal.Transfer" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.10.5/font/bootstrap-icons.min.css" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            /*#btnOpenModal {
                width: 100%;
            }
*/
            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        /* Container for the progress bar */
        .progress-bar-container {
            width: 100%; /* Makes the container take up the full width */
            /*max-width: 600px;*/ /* Optional: sets a maximum width for larger screens */
            background-color: #e0e0e0;
            border-radius: 0px;
            overflow: hidden;
            position: relative;
            margin-top: -20px; /* Adjusts spacing from the <hr /> */
            margin-bottom: 20px; /* Optional: adds some space after the progress bar */
        }

        /* The animated progress bar */
        .progress-bar {
            height: 5px;
            background: linear-gradient(to right, #4caf50, #81c784, #4caf50); /* Green gradient */
            width: 0%; /* Starts from 0% width */
            animation: progress-animation 2s infinite; /* Animation for progress effect */
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Style the autocomplete dropdown to look like a DropDownList */
        .ui-autocomplete {
            list-style: none;
            margin: 0;
            padding: 0;
            background-color: white;
            border: 1px solid #ccc;
            border-radius: 4px;
            max-height: 200px;
            overflow-y: auto;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            z-index: 1050;
            width: 50px;
        }

        .ui-menu-item {
            padding: 8px 12px;
            font-size: 14px;
        }

            .ui-menu-item:hover {
                background-color: #007bff;
                color: white;
            }

        /* Hide any default message shown by jQuery UI */
        .ui-helper-hidden-accessible {
            display: none;
        }

        /* Common alert styles */
        .alert-box {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1050; /* Ensure it stays on top */
            max-width: 500px;
            width: 100%;
            background-color: white;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 5px;
            padding: 20px;
        }

        /* Common alert backdrop */
        .alert-backdrop {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            z-index: 1049; /* Just below the alert */
        }

        /*body {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 100vh;
            background: linear-gradient(to right, #ece9e6, #ffffff);
            font-family: Arial, sans-serif;
        }*/

        .loader-container {
            display: none; /* Initially hidden */
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1051; /* Ensure it stays on top */
            flex-direction: column;
            align-items: center;
        }

        .loader {
            width: 80px;
            height: 80px;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            margin-bottom: 20px;
        }

            .loader::before, .loader::after {
                content: "";
                position: absolute;
                border-radius: 50%;
                animation: pulse 1.5s infinite ease-in-out;
            }

            .loader::before {
                width: 80px;
                height: 80px;
                border: 6px solid #ff7eb3;
            }

            .loader::after {
                width: 60px;
                height: 60px;
                border: 6px solid #ff758c;
                animation-delay: 0.75s;
            }

        @keyframes pulse {
            0% {
                transform: scale(1);
                opacity: 1;
            }

            50% {
                transform: scale(1.3);
                opacity: 0.5;
            }

            100% {
                transform: scale(1);
                opacity: 1;
            }
        }

        .loading-text {
            font-size: 18px;
            color: #333;
            font-weight: bold;
            text-align: center;
        }

        /*Floating Label*/
        .floating-label {
            position: relative;
            /* margin-top: 1.5rem; */
            /*margin-bottom: 1.5rem;*/
        }

            .floating-label label {
                position: absolute;
                top: -0.6rem;
                left: 0.75rem;
                background: white;
                padding: 0 4px;
                font-size: 0.8rem;
                color: #3f51b5;
                z-index: 1;
            }

            .floating-label select,
            .floating-label input,
            .floating-label button,
            .floating-label .aspNetDropDown {
                width: 100%;
                /* padding: 0.6rem 0.75rem; */
                font-size: 1rem;
                border: 1px solid #ccc;
                border-radius: 4px;
                background-color: transparent;
                /*appearance: none;*/
                /*padding: 0.6rem 0.75rem;*/
                position: relative;
                z-index: 0;
            }

            .floating-label button {
                text-align: left;
            }

            /* Optional: prevent pointer cursor on readonly textbox */
            .floating-label input[readonly] {
                background-color: #f9f9f9;
                cursor: default;
            }

            .floating-label select:focus,
            .floating-label select:not([value=""]) {
                outline: none;
            }

                .floating-label select:focus + label,
                .floating-label select:not([value=""]) + label {
                    top: -0.6rem;
                    font-size: 0.8rem;
                    color: #3f51b5;
                }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }
    </script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="loader-container" id="loaderContainer">
        <div class="loader"></div>
        <div class="loading-text" id="loadingText">Loading...</div>
    </div>

    <div class="container body-content">
        <h2 style="text-align: center; margin-top: 20px;">Transfer</h2>

        <div class="headtag">
            <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
        </div>

        <br />
        <br />

        <div class="container">
            <div class="row">
                <div class="col-6 col-md-4 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="StateDrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="StateDrp_SelectedIndexChanged" ClientIDMode="Static">
                        </asp:DropDownList>

                        <input type="text" id="StateSearch" runat="server" class="form-control" placeholder="Enter State" />
                        <label for="StateDrp">State</label>
                        <%--<asp:Label runat="server" Text="From SO" AssociatedControlID="FromSODrp" />--%>
                    </div>
                </div>
                <div class="col-6 col-md-4 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="AreaDrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="AreaDrp_SelectedIndexChanged">
                            <%--<asp:ListItem Text="Area" Value=""></asp:ListItem>--%>
                        </asp:DropDownList>
                        <%--<label for="ToSODrp">To SO</label>--%>
                        <asp:Label runat="server" Text="Area" AssociatedControlID="AreaDrp" />

                        <input type="text" id="AreaSearch" runat="server" class="form-control" placeholder="Enter Area" />
                    </div>
                </div>
                <div class="col-12 col-md-4 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="FromSODrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control" OnSelectedIndexChanged="FromSODrp_SelectedIndexChanged">
                            <%--<asp:ListItem Text="From SO" Value=""></asp:ListItem>--%>
                        </asp:DropDownList>
                        <%--<label for="FromSODrp">From SO</label>--%>
                        <asp:Label runat="server" Text="From SO" AssociatedControlID="FromSODrp" />

                        <input type="text" id="FromSOSearch" runat="server" class="form-control" placeholder="Enter From SO" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-6 col-md-4 mb-2 mb-md-0">
                    <div class="floating-label">
                        <button type="button" style="text-align: center;" class="form-control" id="btnOpenModal" runat="server" data-toggle="modal" data-target="#exampleModalCenter">
                            Distributor(s)
                        </button>
                        <%--<label for="btnOpenModal">Distributor(s)</label>--%>
                        <asp:Label runat="server" Text="Distributor(s)" AssociatedControlID="btnOpenModal" />
                    </div>
                </div>
                <div class="col-6 col-md-4 mb-2 mb-md-0">
                    <div class="floating-label">
                        <asp:DropDownList ID="ToSODrp" runat="server" AutoPostBack="true" Style="display: none;" class="form-control">
                            <asp:ListItem Text="To SO" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <%--<label for="ToSODrp">To SO</label>--%>

                        <asp:Label runat="server" Text="To SO" AssociatedControlID="ToSODrp" />
                        <input type="text" id="ToSODrpSearch" runat="server" class="form-control" placeholder="Enter To SO" />
                    </div>
                </div>
                <div class="col-12 col-md-4 mb-2 mb-md-0">
                    <asp:Button ID="ApproveRowBtn" runat="server" Text="Transfer" CssClass="form-control btn btn-success" OnClientClick="showLoader()" />
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-4 col-md-4 mb-2 mb-md-0">
                    <div class="floating-label">
                        <%-- <asp:DropDownList ID="ECOMRtrDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoader(); this.form.submit();">
                            <asp:ListItem Text="ECOM Chain Group" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <label for="ECOMRtrDrp">ECOM Chain Group</label>--%>
                    </div>

                </div>
                <div class="col-4 col-md-4 mb-2 mb-md-0">
                    <%--<asp:Button ID="FetchToSaveBtn" runat="server" Text="Fetch To Save" CssClass="btn btn-primary form-control" OnClientClick="showLoader()" OnClick="FetchToSaveBtn_Click" />--%>
                </div>

            </div>


            <div class="row">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <%--<asp:GridView ID="FetchtoSaveGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;"
                            OnRowDataBound="FetchtoSaveGrid_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Hierarchy Level Name" DataField="HierarchyLevelName" />
                                <asp:TemplateField HeaderText="Brand">
                                    <ItemTemplate>
                                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("BrandName") %>'></asp:Label>
                                        <asp:Label ID="Label10" runat="server" Text='<%# Bind("BrandID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>--%>
                    </div>
                </div>
            </div>

            <%-- Modal for Distributor(s) --%>
            <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLongTitle">Distributors(s)</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                            <div class="form-group d-flex justify-content-center align-items-center">
                                <input type="text" id="txtSearch" class="form-control" placeholder="Search..." />
                            </div>
                            <div class="form-group">
                                <asp:GridView ID="DistModal" runat="server" AutoPostBack="True" CssClass="table table-bordered form-group"
                                    AutoGenerateColumns="false" DataKeyNames="" Style="margin-bottom: -18px; text-align: center">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div style="margin-right: 10px;">
                                                    <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input rowCheckbox" style="margin-left: -3px;"
                                                        onclick="handleCheckboxClick(this)" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="DistName" HeaderText="Distibutor" />
                                    </Columns>
                                    <HeaderStyle CssClass="header-hidden" />
                                    <RowStyle CssClass="fixed-height-row" BackColor="#FFFFFF" />
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <%--<button type="button" class="btn btn-primary" onclick="selectItems()" data-dismiss="modal">Select</button>--%>
                            <asp:Button ID="SelectBtn" runat="server" Text="Select" CssClass="btn btn-primary" OnClick="SelectBtn_Click" />
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
        <asp:HiddenField ID="hdnBusinessType" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" />
        <asp:HiddenField ID="hfSelectedRowData" runat="server" />
    </div>

    <%--Script for Dropdown Auto Search--%>
    <script>
        $(document).ready(function () {

            // --- State autocomplete ---
            (function () {
                var ddl = $('#<%= StateDrp.ClientID %>');
                var searchBox = $('#<%= StateSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        searchBox.val(ui.item.label);
                        ddl.val(ui.item.value);

                        // Trigger SelectedIndexChanged of StateDrp
                        __doPostBack('<%= StateDrp.UniqueID %>', '');

                        return false;
                    }
                });
            })();


            // --- Area autocomplete ---
            (function () {
                var ddl = $('#<%= AreaDrp.ClientID %>');
                var searchBox = $('#<%= AreaSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        searchBox.val(ui.item.label);
                        ddl.val(ui.item.value);

                        // Trigger SelectedIndexChanged of AreaDrp
                        __doPostBack('<%= AreaDrp.UniqueID %>', '');

                        return false;
                    }
                });
            })();

            // --- FromSO autocomplete ---
            (function () {
                var ddl = $('#<%= FromSODrp.ClientID %>');
                var searchBox = $('#<%= FromSOSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        // set visible text
                        searchBox.val(ui.item.label);
                        // set hidden dropdown selected value
                        ddl.val(ui.item.value);
                        // 🔴 this is what fires FromSODrp_SelectedIndexChanged
                        __doPostBack('<%= FromSODrp.UniqueID %>', '');
                        return false;
                    }
                });
            })();

            // --- ToSODrp autocomplete ---
            (function () {
                var ddl = $('#<%= ToSODrp.ClientID %>');
                var searchBox = $('#<%= ToSODrpSearch.ClientID %>');

                var options = [];
                ddl.find("option").each(function () {
                    var text = $(this).text();
                    var value = $(this).val();
                    if (value) {
                        options.push({ label: text, value: value });
                    }
                });

                searchBox.autocomplete({
                    source: options,
                    minLength: 1,
                    select: function (event, ui) {
                        // set visible text
                        searchBox.val(ui.item.label);
                        // set hidden dropdown selected value
                        ddl.val(ui.item.value);
                        // 🔴 this is what fires ToSODrp_SelectedIndexChanged
                        __doPostBack('<%= ToSODrp.UniqueID %>', '');
            return false;
        }
    });
            })();


        });
    </script>

    <%-- Script for search button in Modal --%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtSearch").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#<%= DistModal.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <%--script for checkboxes in Distributor Modal to remain atleast one Distributor--%>
    <script type="text/javascript">

        // Function to handle checkbox click
        function handleCheckboxClick(checkbox) {
            // Get all checkboxes
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            // Count checked checkboxes
            const checkedCount = Array.from(checkboxes).filter(cb => cb.checked).length;

            // Apply the logic to ensure at least one checkbox remains unchecked
            checkboxes.forEach(cb => {
                if (!cb.checked && checkedCount === checkboxes.length - 1) {
                    cb.disabled = true; // Disable the last unchecked checkbox

                    // Show toast notification
                    showToast("At least one Distributor will remain in Transfer case", "toast-danger");

                } else {
                    cb.disabled = false; // Enable others
                }
            });
        }

        // This function is triggered when the page or modal is loaded to ensure proper checkbox states
        function setCheckboxState() {
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            if (checkboxes.length === 1) {
                // Disable the checkbox if there's only one checkbox in the modal
                checkboxes[0].disabled = true;
            } else {
                // Enable all checkboxes if there are multiple checkboxes
                checkboxes.forEach(cb => cb.disabled = false);
            }
        }

        // Ensure proper checkbox state on modal open
        window.onload = function () {
            setCheckboxState();
        }
    </script>

    <script>
        const messages = [
            "Please wait...",
            "Processing your request...",
            "Almost there...",
            "Just a moment...",
            "Fetching data...",
            "Hang tight...",
            "Thanks for your patience...",
            "We appreciate your wait..."
        ];

        let index = 0;
        let interval;

        function showLoader() {
            document.getElementById("loaderContainer").style.display = "flex"; // Show loader

            // Start changing messages
            interval = setInterval(() => {
                document.getElementById("loadingText").innerText = messages[index];
                index = (index + 1) % messages.length;
            }, 3000);

            // Simulate some process (hide loader after 10s for demo)
            //setTimeout(() => {
            //    hideLoader();
            //}, 10000);
        }

        function hideLoader() {
            clearInterval(interval);
            document.getElementById("loaderContainer").style.display = "none"; // Hide loader
        }
    </script>

</asp:Content>

