--- Datos de contrato
SELECT c.Id, c.FechaInicio, c.FechaFin, c.InmuebleId, c.InquilinoId, c.GaranteId,
--- Datos del Inquilino
i.Id, i.DNI, i.Apellido, 
--- Datos del Garante
g.Id, g.DNI, g.Apellido,
--- Datos del Inmueble
m.Id ,m.Direccion, m.Precio, m.Uso
FROM Contrato c INNER JOIN Inquilino i ON c.InquilinoId = i.Id
INNER JOIN Inmueble m ON c.InmuebleId = m.Id
INNER JOIN Garante g ON c.GaranteId = g.Id;
