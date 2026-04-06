const API_URL = "http://localhost:5283/api/incidents";
// Si tu API corre en otro puerto, cámbialo aquí.

const form = document.getElementById("incident-form");
const incidentList = document.getElementById("incident-list");
const refreshBtn = document.getElementById("refresh-btn");

async function loadIncidents() {
  incidentList.innerHTML = "<p>Loading incidents...</p>";

  try {
    const response = await fetch(API_URL);
    const incidents = await response.json();

    if (!incidents.length) {
      incidentList.innerHTML = "<p>No incidents found.</p>";
      return;
    }

    incidentList.innerHTML = incidents
      .map(
        (incident) => `
          <div class="incident-item">
            <h3>${incident.title}</h3>
            <div class="meta">
              <span class="badge">Severity: ${incident.severity}</span>
              <span class="badge">Status: ${incident.status}</span>
            </div>
            <p>${incident.description ?? "No description"}</p>
            <small>ID: ${incident.id}</small>
          </div>
        `
      )
      .join("");
  } catch (error) {
    incidentList.innerHTML = "<p>Could not load incidents. Check that the API is running.</p>";
    console.error(error);
  }
}

form.addEventListener("submit", async (e) => {
  e.preventDefault();

  const payload = {
    title: document.getElementById("title").value,
    description: document.getElementById("description").value,
    severity: document.getElementById("severity").value
  };

  try {
    const response = await fetch(API_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(payload)
    });

    if (!response.ok) {
      const error = await response.json();
      alert(error.message || "Could not create incident");
      return;
    }

    form.reset();
    loadIncidents();
  } catch (error) {
    alert("Error connecting to the API");
    console.error(error);
  }
});

refreshBtn.addEventListener("click", loadIncidents);

loadIncidents();