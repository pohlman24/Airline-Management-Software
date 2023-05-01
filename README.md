<h1>Air3350</h1>
<p>Completed project that has fully populated tables ready to be test.<br>Group members: Reece Pohlman, Dominic DiPofim Dharsan Guruparan
<h2>Running the Program</h2>
<p>Inside the zip folder containing this document, there is another folder called <em>Air3350</em> containing the executable file named <b>Airline Software.exe</b> along with a few other files. Do not move any of the files outside of this folder or the program will not run properly.

<h2>Database</h2>
<p>We chosed to use CSV files for our database. The way it works is that each csv file acts a database table the first row of the file is the table headers and all following rows are the table records.<br>Due to the lack of white space in CSV files, it can be difficult to read the raw file so you may want to use a tool such as <a href="https://www.convertcsv.com/csv-viewer-editor.htm" target="_blank">this</a> website to view the tables easier. You can copy and paste the table data into the cell<br>WARNING: do not modify the CSV files directly else risk breaking the program.
  
<h2>Technical Notes</h2>
<ul>
  <li>All database "tables" can be found under the 'Tables' folder. Each table is named in the following way <em>class</em>Db.csv</li>
  <li>There are ~1600 flights in the database right now that are scheduled to depart some time this week(04/30/2023)</li>
  <li>Program.cs has the 'main' function that actually outputs the code. We have a few dev functions that can only be used if devmode is turned on via hard coding it. There were making testing easier and generating data</li></ul>
  
<h2>Admins</h2>
<p>All admins currently have the same password, "admin", though they can change it once they log in. Another thing to note is that the database tables for Users will display the hashed password so you can't use that to view the passowrds<p>
<table>
    <thead>
      <th>Role</th>
      <th>Login ID</th>
      <th>Password</th>
      <th>Description</th>
    </thead>
    <tbody>
      <tr>
        <td>Load Engineer</td>
        <td>111111</td>
        <td>admin</td>
        <td>Manage all posted flights</td>
      </tr>
      <tr>
        <td>Flight Manager</td>
        <td>222222</td>
        <td>admin</td>
        <td>Generate Flight Manifest</td>
      </tr>
      <tr>
        <td>Marketing Manager</td>
        <td>333333</td>
        <td>admin</td>
        <td>Assign plane for flights</td>
      </tr>
      <tr>
        <td>Accountant</td>
        <td>444444</td>
        <td>admin</td>
        <td>Calculate various information such as flight income, capacity, and number of departures</td>
      </tr>
    <tbody>
</table>

<h2>Airports</h2>
<table>
  <thead>
    <th>City</th>
    <th>State</th>
    <th>Code</th>
  </thead>
  <tbody>
    <tr>
      <td>Toledo</td>
      <td>OH</td>
      <td>TOL</td>
    </tr>
    <tr>
      <td>Detroit</td>
      <td>MI</td>
      <td>DET</td>
    </tr>
    <tr>
      <td>Nashville</td>
      <td>TN</td>
      <td>BNA</td>
    </tr>
    <tr>
      <td>Los Angeles</td>
      <td>CA</td>
      <td>LAX</td>
    </tr>
    <tr>
      <td>New York</td>
      <td>NY</td>
      <td>JFK</td>
    </tr>
    <tr>
      <td>Chicago</td>
      <td>IL</td>
      <td>ORD</td>
    </tr>
    <tr>
      <td>Atlanta</td>
      <td>GA</td>
      <td>ATL</td>
    </tr>
    <tr>
      <td>Dallas</td>
      <td>TX</td>
      <td>DFW</td>
    </tr>
    <tr>
      <td>Denvor</td>
      <td>CO</td>
      <td>DEN</td>
    </tr>
    <tr>
      <td>Seattle</td>
      <td>WA</td>
      <td>SEA</td>
    </tr>
  </tbody>
</table>
