SELECT c.Id, c.FechaInicio, c.FechaFin, c.InmuebleId, c.InquilinoId, c.GaranteId,
                                i.DNI, i.Apellido,
                                g.DNI, g.Apellido,
                                m.Direccion, m.Precio, m.Uso
                                FROM Contrato c INNER JOIN Inquilino i ON c.InquilinoId = i.Id
                                INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                                INNER JOIN Garante g ON c.GaranteId = g.Id
                                WHERE c.Id=2;
